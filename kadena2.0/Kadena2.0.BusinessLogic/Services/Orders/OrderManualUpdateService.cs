using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using Kadena.Models.CampaignData;
using Kadena.Models.OrderDetail;
using Kadena.Models.Orders;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.MicroserviceRequests;
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
        private readonly IOrderItemCheckerService orderChecker;
        private readonly IProductsService products;
        private readonly ITaxEstimationServiceClient taxes;
        private readonly IKenticoResourceService resources;
        private readonly IDeliveryEstimationDataService deliveryData;
        private readonly IKenticoLogger log;
        private readonly IMapper mapper;
        private readonly IkenticoUserBudgetProvider budgetProvider;
        private readonly IDistributorShoppingCartService distributorShoppingCartService;
        private readonly IShoppingCartProvider shoppingCartProvider;

        public OrderManualUpdateService(IOrderManualUpdateClient updateService,
                                        IOrderViewClient orderService,
                                        IApproverService approvers,
                                        IKenticoProductsProvider productsProvider,
                                        IKenticoSkuProvider skuProvider,
                                        IOrderItemCheckerService orderChecker,
                                        IProductsService products,
                                        ITaxEstimationServiceClient taxes,
                                        IKenticoResourceService resources,
                                        IDeliveryEstimationDataService deliveryData,
                                        IKenticoLogger log,
                                        IMapper mapper,
                                        IDistributorShoppingCartService distributorShoppingCartService,
                                        IShoppingCartProvider shoppingCartProvider,
                                        IkenticoUserBudgetProvider budgetProvider)
        {
            this.updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));
            this.orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            this.approvers = approvers ?? throw new ArgumentNullException(nameof(approvers));
            this.productsProvider = productsProvider ?? throw new ArgumentNullException(nameof(productsProvider));
            this.skuProvider = skuProvider ?? throw new ArgumentNullException(nameof(skuProvider));
            this.orderChecker = orderChecker ?? throw new ArgumentNullException(nameof(orderChecker));
            this.products = products ?? throw new ArgumentNullException(nameof(products));
            this.taxes = taxes ?? throw new ArgumentNullException(nameof(taxes));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.deliveryData = deliveryData ?? throw new ArgumentNullException(nameof(deliveryData));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.distributorShoppingCartService = distributorShoppingCartService ?? throw new ArgumentNullException(nameof(distributorShoppingCartService));
            this.shoppingCartProvider = shoppingCartProvider ?? throw new ArgumentNullException(nameof(shoppingCartProvider));
            this.budgetProvider = budgetProvider ?? throw new ArgumentNullException(nameof(budgetProvider));
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

            var targetAddress = mapper.Map<AddressDto>(orderDetail.ShippingInfo.AddressTo);
            targetAddress.Country = orderDetail.ShippingInfo.AddressTo.isoCountryCode;

            var requestDto = new OrderManualUpdateRequestDto();

            if (orderDetail.Type == OrderType.generalInventory)
            {
                var skuLines = updatedItemsData.ToDictionary(k => k.OriginalItem.SkuId, v => v.UpdatedItem.LineNumber);
                var skuNewQty = updatedItemsData.ToDictionary(k => k.OriginalItem.SkuId, v => v.UpdatedItem.Quantity);

                // create distributor cart items
                var distributorCartItems = distributorShoppingCartService
                    .CreateCart(skuNewQty, orderDetail.Customer.KenticoUserID, orderDetail.campaign.DistributorID)
                    .ToList();
                // create fake cart with new data
                // distributor set to 0 so cart won't be visible for active users
                var cartId = shoppingCartProvider.CreateDistributorCart(0,
                    orderDetail.campaign.ID, orderDetail.campaign.ProgramID,
                    orderDetail.Customer.KenticoUserID);
                // update fake cart
                try
                {
                    distributorCartItems.ForEach(c =>
                    {
                        c.Items.ForEach(i => i.ShoppingCartID = cartId);
                        distributorShoppingCartService.UpdateDistributorCarts(c, orderDetail.Customer.KenticoUserID);
                    }
                    );

                    // get updated data from cart
                    var cart = shoppingCartProvider.GetShoppingCart(cartId, orderDetail.Type);
                    requestDto = mapper.Map<OrderManualUpdateRequestDto>(cart);
                    requestDto.OrderId = request.OrderId;
                    requestDto.Items = cart.Items.Select(i =>
                    {
                        var item = mapper.Map<ItemUpdateDto>(i);
                        item.LineNumber = skuLines[i.SkuId];
                        return item;
                    })
                    .ToList();

                    var weight = shoppingCartProvider.GetCartWeight(cartId);
                    var shippingCost = GetShippinCost(orderDetail.ShippingInfo.Provider, orderDetail.ShippingInfo.ShippingService,
                        weight, targetAddress);
                    requestDto.TotalShipping = shippingCost;
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

                // Adjust budget
                budgetProvider.UpdateUserBudgetAllocationRecords(orderDetail.Customer.KenticoUserID,
                    orderDetail.OrderDate.Year.ToString(),
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

            inventoryProductsData.ForEach(data =>
            {
                var addedQuantity = data.UpdatedItem.Quantity - data.OriginalItem.Quantity;

                // Not using Set... because when waiting for result of OrderUpdate, quantity can change
                skuProvider.UpdateAvailableQuantity(data.Sku.SkuId, addedQuantity);
            });
        }

        void AdjustAvailableItems(IEnumerable<UpdatedItemCheckData> updateData)
        {
            updateData.ToList().ForEach(data =>
            {
                var freedQuantity = data.OriginalItem.Quantity - data.UpdatedItem.Quantity;
                skuProvider.UpdateAvailableQuantity(data.OriginalItem.SkuId, freedQuantity);
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

            var sourceAddress = deliveryData.GetSourceAddress();


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

            request.TotalTax = await EstimateTax(request.TotalPrice, request.TotalShipping, sourceAddress, targetAddress);
        }

        private decimal GetShippinCost(string provider, string shippingService, decimal shippableWeight, AddressDto targetAddress)
        {
            log.LogInfo("Approval", "Info", $"Going to call estimation microservice");

            return deliveryData.GetShippingCost(provider, shippingService, shippableWeight, targetAddress);
        }

        async Task<decimal> EstimateTax(decimal totalBasePrice, decimal shipppingCosts, AddressDto sourceAddress, AddressDto targetAddress)
        {
            if (totalBasePrice == 0.0m)
            {
                return 0.0m;
            }

            var taxRequest = new TaxCalculatorRequestDto
            {
                ShipCost = (double)shipppingCosts,
                TotalBasePrice = (double)totalBasePrice,

                ShipFromCity = sourceAddress.City,
                ShipFromState = sourceAddress.State,
                ShipFromZip = sourceAddress.Postal,

                ShipToCity = targetAddress.City,
                ShipToState = targetAddress.State,
                ShipToZip = targetAddress.Postal
            };

            var taxResult = await taxes.CalculateTax(taxRequest);

            if (!taxResult.Success)
            {
                throw new Exception("Failed to estimate tax");
            }

            return taxResult.Payload;
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
