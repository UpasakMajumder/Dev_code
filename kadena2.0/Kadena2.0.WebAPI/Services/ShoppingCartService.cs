using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Kadena.WebAPI.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.Models.Checkout;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMapper mapper;
        private readonly IKenticoProviderService kenticoProvider;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoLogger kenticoLog;
        private readonly ITaxEstimationService taxCalculator;
        private readonly ITemplatedProductService templateService;

        public ShoppingCartService(IMapper mapper, 
                                   IKenticoProviderService kenticoProvider,
                                   IKenticoResourceService resources,                                    
                                   ITaxEstimationService taxCalculator,
                                   ITemplatedProductService templateService,
                                   IKenticoLogger kenticoLog)
        {
            this.mapper = mapper;
            this.kenticoProvider = kenticoProvider;
            this.resources = resources;            
            this.taxCalculator = taxCalculator;            
            this.templateService = templateService;
            this.kenticoLog = kenticoLog;
        }

        public async Task<CheckoutPage> GetCheckoutPage()
        {
            var addresses = kenticoProvider.GetCustomerAddresses("Shipping");
            var carriers = kenticoProvider.GetShippingCarriers();
            var paymentMethods = kenticoProvider.GetPaymentMethods();
            var cartItems = kenticoProvider.GetShoppingCartItems();
            var items = cartItems.Length == 1 ? "item" : "items"; // todo configurable

            var checkoutPage = new CheckoutPage()
            {
                Products = new CartItems()
                {
                    Number = $"You have {cartItems.Length} {items} in your shopping cart",
                    Items = cartItems.ToList()
                },

                DeliveryAddresses = new DeliveryAddresses()
                {
                    IsDeliverable = true,
                    UnDeliverableText = resources.GetResourceString("Kadena.Checkout.UndeliverableText"),
                    NewAddress = new NewAddressButton()
                    {
                        Label = resources.GetResourceString("Kadena.Checkout.NewAddress"),
                        Url = "/settings?tab=t4"
                    },
                    Title = resources.GetResourceString("Kadena.Checkout.DeliveryAddress.Title"),
                    Description = resources.GetResourceString("Kadena.Checkout.DeliveryDescription"),
                    EmptyMessage = resources.GetResourceString("Kadena.Checkout.NoAddressesMessage"),
                    items = addresses.ToList()
                },

                DeliveryMethods = new DeliveryCarriers()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Delivery.Title"),
                    Description = resources.GetResourceString("Kadena.Checkout.DeliveryMethodDescription"),
                    items = carriers.ToList()
                },

                Totals = new Totals()
                {
                    Title = kenticoProvider.UserCanSeePrices() ? resources.GetResourceString("Kadena.Checkout.Totals.Title") : string.Empty,
                    Description = null, // resources.GetResourceString("Kadena.Checkout.Totals.Description"), if needed
                    // will be assigned after computing TotalTax, which may be changed aftes selecting default shipping or address
                },

                PaymentMethods = new PaymentMethods()
                {
                    IsPayable = true,
                    UnPayableText = resources.GetResourceString("Kadena.Checkout.UnpayableText"),
                    Title = resources.GetResourceString("Kadena.Checkout.Payment.Title"),
                    Description = null, // resources.GetResourceString("Kadena.Checkout.Payment.Description"), if needed
                    Items = ArrangePaymentMethods(paymentMethods)
                },

                Submit = new SubmitButton()
                {
                    BtnLabel = resources.GetResourceString("Kadena.Checkout.ButtonPlaceOrder"),
                    DisabledText = resources.GetResourceString("Kadena.Checkout.ButtonWaitingForTemplateService"),
                    IsDisabled = cartItems.Any(i => i.DesignFilePathRequired && !i.DesignFilePathObtained)
                },
                
                ValidationMessage = resources.GetResourceString("Kadena.Checkout.ValidationError")
            };

            checkoutPage.DeliveryMethods.RemoveCarriersWithoutOptions();
            CheckCurrentOrDefaultAddress(checkoutPage);
            CheckCurrentOrDefaultShipping(checkoutPage);
            checkoutPage.PaymentMethods.CheckDefault();
            checkoutPage.PaymentMethods.CheckPayability();
            checkoutPage.DeliveryMethods.UpdateSummaryText(
                    resources.GetResourceString("Kadena.Checkout.ShippingPriceFrom"),
                    resources.GetResourceString("Kadena.Checkout.ShippingPrice"),
                    resources.GetResourceString("Kadena.Checkout.CannotBeDelivered"),
                    resources.GetResourceString("Kadena.Checkout.CustomerPrice")
                );
            checkoutPage.SetDisplayType();
            SetPricesVisibility(checkoutPage);

            if (kenticoProvider.UserCanSeePrices())
            {
                var totals = kenticoProvider.GetShoppingCartTotals();
                totals.TotalTax = await taxCalculator.EstimateTotalTax();
                checkoutPage.Totals.Items = MapTotals(totals);
            }
            

            return checkoutPage;
        }

        private List<Total> MapTotals(ShoppingCartTotals totals)
        {
            return new Total[]
            {
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Summary"),
                    Value = String.Format("$ {0:#,0.00}", totals.TotalItemsPrice)
                },
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Shipping"),
                    Value = String.Format("$ {0:#,0.00}", totals.TotalShipping)
                },
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Subtotal"),
                    Value = String.Format("$ {0:#,0.00}", totals.Subtotal)
                },
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Tax"),
                    Value = String.Format("$ {0:#,0.00}", totals.TotalTax)
                },
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Totals"),
                    Value = String.Format("$ {0:#,0.00}", totals.TotalPrice)
                }
            }.ToList();
        }

        private void CheckCurrentOrDefaultAddress(CheckoutPage page)
        {
            int currentAddress = kenticoProvider.GetCurrentCartAddresId();
            if (currentAddress != 0 && page.DeliveryAddresses.items.Any(a => a.Id == currentAddress))
            {
                page.DeliveryAddresses.CheckAddress(currentAddress);
            }
            else
            {
                int defaultAddressId = page.DeliveryAddresses.GetDefaultAddressId();
                if (defaultAddressId != 0)
                {
                    kenticoProvider.SetShoppingCartAddres(defaultAddressId);
                    page.DeliveryAddresses.CheckAddress(defaultAddressId);
                }
            }
        }

        private void CheckCurrentOrDefaultShipping(CheckoutPage page)
        {
            int currentShipping = kenticoProvider.GetCurrentCartShippingOptionId();

            if (page.DeliveryMethods.IsPresent(currentShipping) && !page.DeliveryMethods.IsDisabled(currentShipping))
            {
                page.DeliveryMethods.CheckMethod(currentShipping);
            }
            else
            {
                SetDefaultShipping(page);
            }
        }

        private void SetDefaultShipping(CheckoutPage page)
        {
            int defaultMethodId = page.DeliveryMethods.GetDefaultMethodId();
            kenticoProvider.SelectShipping(defaultMethodId);
            page.DeliveryMethods.CheckMethod(defaultMethodId);
        }

        private void SetPricesVisibility(CheckoutPage page)
        {
            if (!kenticoProvider.UserCanSeePrices())
            {
                page.DeliveryMethods.HidePrices();

                page.Products.HidePrices();
            }
        }

        private List<PaymentMethod> ArrangePaymentMethods(PaymentMethod[] allMethods)
        {
            var purchaseOrderMethod = allMethods.Where(m => m.ClassName.Contains("PurchaseOrder")).FirstOrDefault();
            if (purchaseOrderMethod != null)
            {
                purchaseOrderMethod.HasInput = true;
                purchaseOrderMethod.InputPlaceholder = resources.GetResourceString("Kadena.Checkout.InsertPONumber");
            }

            return allMethods.ToList();
        }

        public async Task<CheckoutPage> SelectShipipng(int id)
        {
            kenticoProvider.SelectShipping(id);
            return await GetCheckoutPage();
        }

        public async Task<CheckoutPage> SelectAddress(int id)
        {
            kenticoProvider.SetShoppingCartAddres(id);
            var checkoutPage = await GetCheckoutPage();
            checkoutPage.DeliveryAddresses.CheckAddress(id);
            return checkoutPage;
        }

        public async Task<CheckoutPage> ChangeItemQuantity(int id, int quantity)
        {
            kenticoProvider.SetCartItemQuantity(id, quantity);
            return await GetCheckoutPage();
        }

        public async Task<CheckoutPage> RemoveItem(int id)
        {
            kenticoProvider.RemoveCartItem(id);
            return await GetCheckoutPage();
        }

        public async Task<bool> IsSubmittable() // TODO not refactored into Order service, will change after implementing microservice to handle pdf creation
        {
            string endpoint = resources.GetSettingsKey("KDA_TemplatingServiceEndpoint");
            var items = kenticoProvider.GetShoppingCartItems().Where(i => i.DesignFilePathRequired && !i.DesignFilePathObtained).ToList();

            foreach(var item in items)// todo consider parallel for-each
            {
                var state = await templateService.GetGeneratePdfTaskStatus(endpoint, item.ChilliEditorTemplateId.ToString(), item.DesignFilePathTaskId);
                if (state.Success && state.Payload!=null) 
                {
                    var payload = state.Payload;
                    if (payload.Finished && payload.Succeeded)
                    {
                        item.DesignFilePath = payload.FileName.ToString();
                        item.DesignFilePathObtained = true;
                        kenticoProvider.SetCartItemDesignFilePath(item.Id, item.DesignFilePath);
                    }
                }
            }

            return items.TrueForAll(i => i.DesignFilePathObtained);
        }
    }
}
 