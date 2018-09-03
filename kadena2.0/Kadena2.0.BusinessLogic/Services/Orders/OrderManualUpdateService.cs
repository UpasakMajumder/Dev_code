using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using Kadena.Models;
using Kadena.Models.CampaignData;
using Kadena.Models.Checkout;
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

            var updateItems = request.Items
                .Join(orderDetail.Items,
                    chi => chi.LineNumber,
                    oi => oi.LineNumber,
                    (chi, oi) => new
                    {
                        oi.LineNumber,
                        oi.DocumentId,
                        AdjustedQuantity = chi.Quantity - oi.Quantity,
                        NewQuantity = chi.Quantity,
                        oi.TemplateId,
                        oi.SkuId
                    })
                .ToList();

            if (updateItems.Count() != request.Items.Count())
            {
                throw new Exception("Couldn't match all given line numbers in original order");
            }

            if (IsCreditCardPayment(orderDetail.PaymentInfo.PaymentMethod) && updateItems.Any(i => i.AdjustedQuantity > 0))
            {
                throw new Exception("Can't increase item quantity, if payment method is credit card.");
            }

            var requestDto = new OrderManualUpdateRequestDto();

            if (orderDetail.Type == OrderType.generalInventory)
            {
                // validate modification
                foreach (var i in updateItems)
                {
                    if (i.AdjustedQuantity > 0)
                    {
                        distributorShoppingCartService.ValidateItem(i.SkuId, i.AdjustedQuantity, orderDetail.Customer.KenticoUserID);
                    }
                }
                // create fake cart with new data
                var taxAddress = mapper.Map<DeliveryAddress>(orderDetail.ShippingInfo.AddressTo);
                var cart = new ShoppingCart
                {
                    CampaignId = orderDetail.campaign.ID,
                    ProgramId = orderDetail.campaign.ProgramID,
                    UserId = orderDetail.Customer.KenticoUserID,
                    ShippingOptionId = orderDetail.ShippingInfo.ShippingOptionId,
                    Address = taxAddress,
                    Items = updateItems
                       .Select(i => new CartItemEntity
                       {
                           SKUID = i.SkuId,
                           Quantity = i.NewQuantity
                       })
                       .ToList()
                };
                cart = shoppingCartProvider.Evaluate(cart);
                try
                {
                    // get updated data from cart
                    requestDto = mapper.Map<OrderManualUpdateRequestDto>(cart, opt => updateItems
                        .ForEach(i => opt.Items.Add(i.SkuId.ToString(), i.LineNumber)));
                    requestDto.OrderId = request.OrderId;
                    requestDto.Items
                        .Concat(updateItems
                            .Where(i => i.NewQuantity < 1)
                            .Select(i => new ItemUpdateDto
                            {
                                LineNumber = i.LineNumber
                            }))
                        .ToList();

                    requestDto.TotalTax = await taxEstimationService.EstimateTax(taxAddress, requestDto.TotalPrice, requestDto.TotalShipping);
                }
                catch (Exception exc)
                {
                    log.LogException(this.GetType().Name, exc);
                }

                // send to microservice
                var updateResult = await updateService.UpdateOrder(requestDto);
                if (!updateResult.Success)
                {
                    throw new Exception("Failed to call order update microservice. " + updateResult.ErrorMessages);
                }

                // adjust available quantity
                updateItems
                    .ForEach(i =>
                    {
                        skuProvider.UpdateAvailableQuantity(i.SkuId, -i.AdjustedQuantity);
                        productsProvider.UpdateAllocatedProductQuantityForUser(i.SkuId, orderDetail.Customer.KenticoUserID, i.AdjustedQuantity);
                    });

                // Adjust budget
                budgetProvider.AdjustUserRemainingBudget(
                    orderDetail.OrderDate.Year.ToString(),
                    orderDetail.Customer.KenticoUserID,
                    requestDto.TotalShipping - Convert.ToDecimal(orderDetail.PaymentInfo.Shipping));
            }
            else
            {
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

                var documentIds = updateItems.Select(i => i.DocumentId).Distinct().ToArray();
                var skuIds = updateItems.Select(i => i.SkuId).Distinct().ToArray();
                var products = productsProvider.GetProductsByDocumentIds(documentIds);
                var skus = skuProvider.GetSKUsByIds(skuIds);

                updatedItemsData.ForEach(d =>
                {
                    var sku = skus.FirstOrDefault(s => s.SkuId == d.OriginalItem.SkuId)
                              ?? throw new Exception($"Unable to find SKU {d.OriginalItem.SkuId} of item {d.OriginalItem.Name}");

                    var product = products.FirstOrDefault(p => p.Id == d.OriginalItem.DocumentId)
                                  ?? throw new Exception($"Unable to find product {d.OriginalItem.DocumentId} of item {d.OriginalItem.Name}");

                    d.Sku = sku;
                    d.Product = product;

                    ValidateItem(product, sku, d.UpdatedItem.Quantity, d.UpdatedItem.Quantity - d.OriginalItem.Quantity);
                    var unitPrice = this.products.GetPriceByCustomModel(d.Product.Id, d.UpdatedItem.Quantity);
                    if (unitPrice == decimal.MinusOne)
                    {
                        unitPrice = sku.Price;
                    }

                    d.ManuallyUpdatedItem = new ItemUpdateDto
                    {
                        LineNumber = d.OriginalItem.LineNumber,
                        Quantity = d.UpdatedItem.Quantity,
                        TotalPrice = Math.Round(unitPrice * d.UpdatedItem.Quantity, 2),
                        UnitPrice = unitPrice
                    };
                });

                requestDto = new OrderManualUpdateRequestDto
                {
                    OrderId = request.OrderId,
                    Items = updatedItemsData.Select(d => d.ManuallyUpdatedItem).ToList()
                };
                var targetAddress = mapper.Map<AddressDto>(orderDetail.ShippingInfo.AddressTo);
                targetAddress.Country = orderDetail.ShippingInfo.AddressTo.isoCountryCode;
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

            inventoryProductsData
                .ToList()
                .ForEach(data =>
                    {
                        var freedQuantity = data.OriginalItem.Quantity - data.UpdatedItem.Quantity;
                        // Not using Set... because when waiting for result of OrderUpdate, quantity can change
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

        void ValidateItem(Product product, Sku sku, int newQuantity, int adjustedQuantity)
        {
            if (ProductTypes.IsOfType(product.ProductType, ProductTypes.MailingProduct))
            {
                throw new Exception("Cannot change quantity of Mailing product item");
            }

            orderChecker.CheckMinMaxQuantity(sku, newQuantity);

            if (ProductTypes.IsOfType(product.ProductType, ProductTypes.InventoryProduct))
            {
                orderChecker.EnsureInventoryAmount(sku, adjustedQuantity, newQuantity);
            }
        }
    }
}
