using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using Kadena.Models;
using Kadena.Models.CampaignData;
using Kadena.Models.OrderDetail;
using Kadena.Models.Orders;
using Kadena.Models.Product;
using Kadena.Models.ShoppingCarts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderManualUpdateService : IOrderManualUpdateService
    {
        private class UpdatedItemCheckData
        {
            public Product Product { get; set; }
            public OrderItemUpdate UpdatedItem { get; set; }
            public Sku Sku { get; set; }
            public OrderItemDTO OriginalItem { get; set; }
            public ItemUpdateDto ManuallyUpdatedItem { get; set; }
        }

        private readonly IOrderManualUpdateClient updateService;
        private readonly IOrderViewClient orderService;
        private readonly IApproverService approvers;
        private readonly IKenticoProductsProvider productsProvider;
        private readonly IKenticoSkuProvider skuProvider;
        private readonly IKenticoPermissionsProvider permissions;
        private readonly IOrderItemCheckerService orderChecker;
        private readonly IProductsService products;
        private readonly IKenticoResourceService resources;
        private readonly IDeliveryEstimationDataService deliveryData;
        private readonly IKenticoLogger log;
        private readonly IMapper mapper;
        private readonly IKenticoUserBudgetProvider budgetProvider;
        private readonly IDistributorShoppingCartService distributorShoppingCartService;
        private readonly IShoppingCartProvider shoppingCartProvider;
        private readonly ITaxEstimationService taxEstimationService;

        public OrderManualUpdateService(IOrderManualUpdateClient updateService,
                                        IOrderViewClient orderService,
                                        IApproverService approvers,
                                        IKenticoProductsProvider productsProvider,
                                        IKenticoSkuProvider skuProvider,
                                        IKenticoPermissionsProvider permissions,
                                        IOrderItemCheckerService orderChecker,
                                        IProductsService products,
                                        IKenticoResourceService resources,
                                        IDeliveryEstimationDataService deliveryData,
                                        IKenticoLogger log,
                                        IMapper mapper,
                                        IDistributorShoppingCartService distributorShoppingCartService,
                                        IShoppingCartProvider shoppingCartProvider,
                                        IKenticoUserBudgetProvider budgetProvider,
                                        ITaxEstimationService taxEstimationService)
        {
            this.updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));
            this.orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            this.approvers = approvers ?? throw new ArgumentNullException(nameof(approvers));
            this.productsProvider = productsProvider ?? throw new ArgumentNullException(nameof(productsProvider));
            this.skuProvider = skuProvider ?? throw new ArgumentNullException(nameof(skuProvider));
            this.permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            this.orderChecker = orderChecker ?? throw new ArgumentNullException(nameof(orderChecker));
            this.products = products ?? throw new ArgumentNullException(nameof(products));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.deliveryData = deliveryData ?? throw new ArgumentNullException(nameof(deliveryData));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.distributorShoppingCartService = distributorShoppingCartService ?? throw new ArgumentNullException(nameof(distributorShoppingCartService));
            this.shoppingCartProvider = shoppingCartProvider ?? throw new ArgumentNullException(nameof(shoppingCartProvider));
            this.budgetProvider = budgetProvider ?? throw new ArgumentNullException(nameof(budgetProvider));
            this.taxEstimationService = taxEstimationService ?? throw new ArgumentNullException(nameof(taxEstimationService));
        }

        public async Task<OrderUpdateResult> UpdateOrder(OrderUpdate request)
        {
            CheckRequestData(request);

            var orderDetailResult = await orderService.GetOrderByOrderId(request.OrderId);

            if (!orderDetailResult.Success || orderDetailResult.Payload == null)
            {
                throw new Exception($"Failed to retireve data for order {request.OrderId}");
            }

            var orderDetail = orderDetailResult.Payload;

            if (orderDetail.Type == OrderType.prebuy)
            {
                throw new InvalidOperationException("Editing of order isn't supported for Pre-buy orders.");
            }



            var itemsWithoutDocument = orderDetail.Items.Where(i => i.DocumentId == 0).Select(i => i.Name);

            if (itemsWithoutDocument.Any())
            {
                throw new Exception("Following items were ordered with empty documentId : " + string.Join(", ", itemsWithoutDocument));
            }

            approvers.CheckIsCustomersEditor(orderDetail.ClientId);


            var updatedItemsData = request.Items.Join(orderDetail.Items,
                                                       chi => chi.LineNumber,
                                                       oi => oi.LineNumber,
                                                       (chi, oi) => new UpdatedItemCheckData
                                                       {
                                                           OriginalItem = oi,
                                                           UpdatedItem = chi
                                                       }
                                                       ).ToList();

            if (updatedItemsData.Count() != request.Items.Count())
            {
                throw new Exception("Couldn't match all given line numbers in original order");
            }

            if (IsCreditCardPayment(orderDetail.PaymentInfo.PaymentMethod) && updatedItemsData.Any(i => i.UpdatedItem.Quantity > i.OriginalItem.Quantity))
            {
                throw new Exception("Can't increase item quantity, if payment method is credit card.");
            }

            var targetAddress = mapper.Map<AddressDto>(orderDetail.ShippingInfo.AddressTo);
            targetAddress.Country = orderDetail.ShippingInfo.AddressTo.isoCountryCode;

            var requestDto = new OrderManualUpdateRequestDto();

            if (orderDetail.Type == OrderType.generalInventory)
            {
                var skuLines = updatedItemsData.ToDictionary(k => k.OriginalItem.SkuId, v => v.UpdatedItem.LineNumber);
                var skuNewQty = updatedItemsData.ToDictionary(k => k.OriginalItem.SkuId, v => v.UpdatedItem.Quantity);
                var skuAdjustedQuantities = updatedItemsData.ToDictionary(k => k.OriginalItem.SkuId, v => v.UpdatedItem.Quantity - v.OriginalItem.Quantity);

                // create distributor cart items
                var distributorCartItems = distributorShoppingCartService
                    .CreateCart(skuAdjustedQuantities, orderDetail.Customer.KenticoUserID, orderDetail.campaign.DistributorID)
                    .ToList();
                // create fake cart with new data
                // distributor set to 0 so cart won't be visible for active users
                var cart = new ShoppingCart
                {
                    CampaignId = orderDetail.campaign.ID,
                    ProgramId = orderDetail.campaign.ProgramID,
                    UserId = orderDetail.Customer.KenticoUserID
                };
                var cartId = shoppingCartProvider.SaveCart(cart);
                // update fake cart
                try
                {
                    distributorCartItems.ForEach(c =>
                    {
                        c.Items.ForEach(i =>
                        {
                            i.ShoppingCartID = cartId;
                            i.Quantity = skuNewQty[c.SKUID];
                        });
                        distributorShoppingCartService.UpdateDistributorCarts(c, orderDetail.Customer.KenticoUserID);
                    }
                    );

                    // deleted items will not be shopping cart, need to add them manually
                    var deletedItems = updatedItemsData
                        .Where(u => u.UpdatedItem.Quantity == 0)
                        .Select(u => new ItemUpdateDto
                        {
                            LineNumber = u.UpdatedItem.LineNumber,
                            Quantity = 0,
                            TotalPrice = 0,
                            UnitPrice = 0
                        });

                    // get updated data from cart
                    cart = shoppingCartProvider.GetShoppingCart(cartId);
                    requestDto = mapper.Map<OrderManualUpdateRequestDto>(cart);
                    requestDto.OrderId = request.OrderId;
                    requestDto.Items = cart.Items
                        .Select(i =>
                        {
                            var item = mapper.Map<ItemUpdateDto>(i);
                            item.LineNumber = skuLines[i.SKUID];
                            return item;
                        })
                        .Concat(deletedItems)
                        .ToList();

                    var weight = shoppingCartProvider.GetCartWeight(cartId);
                    var shippingCost = GetShippinCost(orderDetail.ShippingInfo.Provider, orderDetail.ShippingInfo.ShippingService,
                        weight, targetAddress);
                    requestDto.TotalShipping = shippingCost;

                    var taxAddress = mapper.Map<DeliveryAddress>(orderDetail.ShippingInfo.AddressTo);
                    requestDto.TotalTax = await taxEstimationService.EstimateTax(taxAddress, requestDto.TotalPrice, requestDto.TotalShipping);
                }
                catch (Exception exc)
                {
                    log.LogException(this.GetType().Name, exc);
                }
                finally
                {
                    // remove fake cart
                    shoppingCartProvider.DeleteShoppingCart(cartId);
                }

                // send to microservice
                var updateResult = await updateService.UpdateOrder(requestDto);
                if (!updateResult.Success)
                {
                    throw new Exception("Failed to call order update microservice. " + updateResult.ErrorMessages);
                }

                // adjust available quantity
                AdjustAvailableItems(updatedItemsData);
                AdjustAllocatedItems(updatedItemsData, orderDetail.Customer.KenticoUserID);

                // Adjust budget
                budgetProvider.AdjustUserRemainingBudget(
                    orderDetail.OrderDate.Year.ToString(),
                    orderDetail.Customer.KenticoUserID,
                    requestDto.TotalShipping - Convert.ToDecimal(orderDetail.PaymentInfo.Shipping));
            }
            else
            {
                var documentIds = orderDetail.Items.Select(i => i.DocumentId).Distinct().ToArray();
                var skuIds = orderDetail.Items.Select(i => i.SkuId).Distinct().ToArray();
                var products = productsProvider.GetProductsByDocumentIds(documentIds);
                var skus = skuProvider.GetSKUsByIds(skuIds);

                updatedItemsData.ForEach(u =>
                {
                    var sku = skus.FirstOrDefault(s => s.SkuId == u.OriginalItem.SkuId)
                              ?? throw new Exception($"Unable to find SKU {u.OriginalItem.SkuId} of item {u.OriginalItem.Name}");

                    var product = products.FirstOrDefault(p => p.Id == u.OriginalItem.DocumentId)
                                  ?? throw new Exception($"Unable to find product {u.OriginalItem.DocumentId} of item {u.OriginalItem.Name}");

                    u.Sku = sku;
                    u.Product = product;
                });

                updatedItemsData.ForEach(d => d.ManuallyUpdatedItem = CreateChangedItem(d));

                var changedItems = updatedItemsData.Select(d => d.ManuallyUpdatedItem).ToList();

                requestDto = new OrderManualUpdateRequestDto
                {
                    OrderId = request.OrderId,
                    Items = changedItems,
                };

                await DoEstimations(requestDto, updatedItemsData, orderDetail, skus, targetAddress);
                var updateResult = await updateService.UpdateOrder(requestDto);
                if (!updateResult.Success)
                {
                    throw new Exception("Failed to call order update microservice. " + updateResult.ErrorMessages);
                }

                UpdateAvailableItems(updatedItemsData);
            }

            return GetUpdatesForFrontend(requestDto);
        }

        void CheckRequestData(OrderUpdate request)
        {
            OrderNumber.Parse(request.OrderId);

            if ((request.Items?.Count() ?? 0) == 0)
            {
                throw new Exception("No items were submitted to process");
            }

            if (request.Items.GroupBy(i => i.LineNumber).Count() != request.Items.Count())
            {
                throw new Exception("Line numbers of changed items are not unique");
            }

            if (request.Items.Any(i => i.Quantity < 0))
            {
                throw new Exception("New item quantity cannot be < 0");
            }
        }

        OrderUpdateResult GetUpdatesForFrontend(OrderManualUpdateRequestDto requestDto)
        {
            if (!permissions.UserCanSeePrices())
            {
                return null;
            }

            var result = new OrderUpdateResult
            {
                PricingInfo = new[]
                {
                        new TitleValuePair<string>
                        {
                            Title = resources.GetResourceString("Kadena.Order.PricingSummary"),
                            Value = String.Format("$ {0:#,0.00}", requestDto.TotalPrice)
                        },
                        new TitleValuePair<string>
                        {
                            Title = resources.GetResourceString("Kadena.Order.PricingShipping"),
                            Value = String.Format("$ {0:#,0.00}", requestDto.TotalShipping)
                        },
                        new TitleValuePair<string>
                        {
                            Title = resources.GetResourceString("Kadena.Order.PricingSubtotal"),
                            Value = String.Format("$ {0:#,0.00}",requestDto.TotalPrice + requestDto.TotalShipping)
                        },
                        new TitleValuePair<string>
                        {
                            Title = resources.GetResourceString("Kadena.Order.PricingTax"),
                            Value = String.Format("$ {0:#,0.00}",requestDto.TotalTax)
                        },
                        new TitleValuePair<string>
                        {
                            Title = resources.GetResourceString("Kadena.Order.PricingTotals"),
                            Value = String.Format("$ {0:#,0.00}",requestDto.TotalPrice + requestDto.TotalShipping + requestDto.TotalTax)
                        }

                },

                OrdersPrice = requestDto.Items.Select(d => new ItemUpdateResult
                {
                    LineNumber = d.LineNumber,
                    Price = String.Format("$ {0:#,0.00}", d.TotalPrice)
                }).ToArray()
            };

            return result;
        }

        void UpdateAvailableItems(IEnumerable<UpdatedItemCheckData> updateData)
        {
            var inventoryProductsData = updateData
                .Where(u => ProductTypes.IsOfType(u.Product.ProductType, ProductTypes.InventoryProduct))
                .ToList();

            AdjustAvailableItems(inventoryProductsData);
        }

        void AdjustAvailableItems(IEnumerable<UpdatedItemCheckData> updateData)
        {
            updateData.ToList().ForEach(data =>
            {
                var freedQuantity = data.OriginalItem.Quantity - data.UpdatedItem.Quantity;
                // Not using Set... because when waiting for result of OrderUpdate, quantity can change
                skuProvider.UpdateAvailableQuantity(data.OriginalItem.SkuId, freedQuantity);
            });
        }

        void AdjustAllocatedItems(IEnumerable<UpdatedItemCheckData> updateData, int userId)
        {
            updateData.ToList().ForEach(data =>
            {
                var adjustedQuantity = data.UpdatedItem.Quantity - data.OriginalItem.Quantity;
                // Not using Set... because when waiting for result of OrderUpdate, quantity can change
                productsProvider.UpdateAllocatedProductQuantityForUser(data.OriginalItem.SkuId, userId, adjustedQuantity);
            });
        }

        async Task DoEstimations(OrderManualUpdateRequestDto request, IEnumerable<UpdatedItemCheckData> updateData, GetOrderByOrderIdResponseDTO orderDetail, Sku[] skus,
            AddressDto targetAddress)
        {
            request.TotalPrice = 0.0m;
            var shippableWeight = 0.0m;

            orderDetail.Items.ForEach(i =>
            {
                var updatedItem = updateData.FirstOrDefault(d => d.ManuallyUpdatedItem.LineNumber == i.LineNumber);

                if (updatedItem != null)
                {
                    request.TotalPrice += updatedItem.ManuallyUpdatedItem.TotalPrice;
                    if (updatedItem.Sku.NeedsShipping)
                    {
                        shippableWeight += (decimal)updatedItem.Sku.Weight * updatedItem.ManuallyUpdatedItem.Quantity;
                    }
                }
                else
                {
                    request.TotalPrice += (decimal)i.TotalPrice;
                    var sku = skus.First(s => s.SkuId == i.SkuId);
                    if (sku.NeedsShipping)
                    {
                        shippableWeight += (decimal)sku.Weight * i.Quantity;
                    }
                }
            }
            );

            request.TotalShipping = 0.0m;

            log.LogInfo("Approval", "Info", $"Provider is '{orderDetail.ShippingInfo.Provider}'");
            log.LogInfo("Approval", "Info", $"Total shippable weight is '{shippableWeight}'");

            if (!orderDetail.ShippingInfo.Provider.EndsWith("Customer") && shippableWeight > 0.0m)
            {
                request.TotalShipping = GetShippinCost(orderDetail.ShippingInfo.Provider, orderDetail.ShippingInfo.ShippingService,
                    shippableWeight, targetAddress);
            }
            else
            {
                log.LogInfo("Approval", "Info", $"NOT going to call estimation microservice");
            }
            var taxAddress = mapper.Map<DeliveryAddress>(orderDetail.ShippingInfo.AddressTo);
            request.TotalTax = await taxEstimationService.EstimateTax(taxAddress, request.TotalPrice, request.TotalShipping);
        }

        private bool IsCreditCardPayment(string paymentMethod)
        {
            return paymentMethod == "CreditCard" || paymentMethod == "CreditCardDemo";
        }

        private decimal GetShippinCost(string provider, string shippingService, decimal shippableWeight, AddressDto targetAddress)
        {
            log.LogInfo("Approval", "Info", $"Going to call estimation microservice");

            return deliveryData.GetShippingCost(provider, shippingService, shippableWeight, targetAddress);
        }

        ItemUpdateDto CreateChangedItem(UpdatedItemCheckData data)
        {
            if (ProductTypes.IsOfType(data.Product.ProductType, ProductTypes.MailingProduct))
            {
                throw new Exception("Cannot change quantity of Mailing product item");
            }

            orderChecker.CheckMinMaxQuantity(data.Sku, data.UpdatedItem.Quantity);

            var addedQuantity = data.UpdatedItem.Quantity - data.OriginalItem.Quantity;

            if (ProductTypes.IsOfType(data.Product.ProductType, ProductTypes.InventoryProduct))
            {
                orderChecker.EnsureInventoryAmount(data.Sku, addedQuantity, data.UpdatedItem.Quantity);
            }

            var unitPrice = products.GetPriceByCustomModel(data.OriginalItem.DocumentId, data.UpdatedItem.Quantity);
            if (unitPrice == decimal.MinusOne)
            {
                unitPrice = data.Sku.Price;
            }

            return new ItemUpdateDto
            {
                LineNumber = data.OriginalItem.LineNumber,
                Quantity = data.UpdatedItem.Quantity,
                TotalPrice = Math.Round(unitPrice * data.UpdatedItem.Quantity, 2),
                UnitPrice = unitPrice
            };
        }
    }
}
