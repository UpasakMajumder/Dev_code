using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Checkout;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Product;
using Kadena.BusinessLogic.Factories.Checkout;

namespace Kadena.BusinessLogic.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMapper mapper;
        private readonly IKenticoProviderService kenticoProvider;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoLogger kenticoLog;
        private readonly ITaxEstimationService taxCalculator;
        private readonly IKListService mailingService;
        private readonly IKenticoDocumentProvider documents;
        private readonly ICheckoutPageFactory checkoutfactory;

        public ShoppingCartService(IMapper mapper,
                                   IKenticoProviderService kenticoProvider,
                                   IKenticoUserProvider kenticoUsers,
                                   IKenticoResourceService resources,
                                   ITaxEstimationService taxCalculator,
                                   IKListService mailingService,
                                   IKenticoDocumentProvider documents,
                                   IKenticoLogger kenticoLog,
                                   ICheckoutPageFactory checkoutfactory)
        {
            this.mapper = mapper;
            this.kenticoProvider = kenticoProvider;
            this.kenticoUsers = kenticoUsers;
            this.resources = resources;
            this.taxCalculator = taxCalculator;
            this.mailingService = mailingService;
            this.documents = documents;
            this.kenticoLog = kenticoLog;
            this.checkoutfactory = checkoutfactory;
        }

        public CheckoutPage GetCheckoutPage()
        {
            var addresses = kenticoUsers.GetCustomerAddresses(AddressType.Shipping);
            var paymentMethods = kenticoProvider.GetPaymentMethods();
            var cartItems = kenticoProvider.GetShoppingCartItems();
            var cartItemsTotals = kenticoProvider.GetShoppingCartTotals();
            var countOfItemsString = cartItems.Length == 1 ? resources.GetResourceString("Kadena.Checkout.ItemSingular") : resources.GetResourceString("Kadena.Checkout.ItemPlural");
            var userNotificationString = GetUserNotificationString();
            var otherAddressEnabled = GetOtherAddressSettingsValue();            

            var checkoutPage = new CheckoutPage()
            {
                EmptyCart = checkoutfactory.CreateCartEmptyInfo(cartItems),
                Products = checkoutfactory.CreateProducts(cartItems, cartItemsTotals, countOfItemsString),
                DeliveryAddresses = checkoutfactory.CreateDeliveryAddresses(addresses.ToList(), userNotificationString, otherAddressEnabled),
                PaymentMethods = checkoutfactory.CreatePaymentMethods(paymentMethods),
                Submit = checkoutfactory.CreateSubmitButton(),
                ValidationMessage = resources.GetResourceString("Kadena.Checkout.ValidationError")
            };

            CheckCurrentOrDefaultAddress(checkoutPage);
            checkoutPage.PaymentMethods.CheckDefault();
            checkoutPage.PaymentMethods.CheckPayability();
            checkoutPage.SetDisplayType();
            SetPricesVisibility(checkoutPage);
            return checkoutPage;
        }
        
        public async Task<CheckoutPageDeliveryTotals> GetDeliveryAndTotals()
        {
            var deliveryAddress = kenticoProvider.GetCurrentCartShippingAddress();

            var isShippingApplicable = kenticoProvider.GetShoppingCartItems()
                .Any(item => !item.IsMailingList);
            if (!isShippingApplicable)
            {
                UnsetShipping();
            }

            var result = new CheckoutPageDeliveryTotals()
            {
                DeliveryMethods = GetDeliveryMethods(isShippingApplicable),
                Totals = new Totals()
                {
                    Title = string.Empty,
                    Description = null // resources.GetResourceString("Kadena.Checkout.Totals.Description"), if needed
                }
            };

            if (kenticoUsers.UserCanSeePrices())
            {
                await UpdateTotals(result, deliveryAddress);
            }

            SetPricesVisibility(result);
            return result;
        }

        public async Task<CheckoutPageDeliveryTotals> SetDeliveryAddress(DeliveryAddress deliveryAddress)
        {
            kenticoProvider.SetShoppingCartAddress(deliveryAddress);
            return await GetDeliveryAndTotals();
        }

        private DeliveryCarriers GetDeliveryMethods(bool isShippingApplicable)
        {
            if (!isShippingApplicable)
            {
                var defaultDeliveryMethods = new DeliveryCarriers();
                return defaultDeliveryMethods;
            }

            var carriers = kenticoProvider.GetShippingCarriers();
            var deliveryMethods = new DeliveryCarriers()
            {
                Title = resources.GetResourceString("Kadena.Checkout.Delivery.Title"),
                Description = resources.GetResourceString("Kadena.Checkout.DeliveryMethodDescription"),
                items = carriers.ToList()
            };

            deliveryMethods.RemoveCarriersWithoutOptions();

            CheckCurrentOrDefaultShipping(deliveryMethods);

            deliveryMethods.UpdateSummaryText(
                resources.GetResourceString("Kadena.Checkout.ShippingPriceFrom"),
                resources.GetResourceString("Kadena.Checkout.ShippingPrice"),
                resources.GetResourceString("Kadena.Checkout.CannotBeDelivered"),
                resources.GetResourceString("Kadena.Checkout.CustomerPrice")
            );

            return deliveryMethods;
        }

        private async Task UpdateTotals(CheckoutPageDeliveryTotals page, DeliveryAddress deliveryAddress)
        {
            var totals = page.Totals;
            totals.Title = resources.GetResourceString("Kadena.Checkout.Totals.Title");
            var shoppingCartTotals = kenticoProvider.GetShoppingCartTotals();
            shoppingCartTotals.TotalTax = await taxCalculator.EstimateTotalTax(deliveryAddress);
            totals.Items = new Total[]
            {
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Summary"),
                    Value = String.Format("$ {0:#,0.00}", shoppingCartTotals.TotalItemsPrice)
                },
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Shipping"),
                    Value = String.Format("$ {0:#,0.00}", shoppingCartTotals.TotalShipping)
                },
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Subtotal"),
                    Value = String.Format("$ {0:#,0.00}", shoppingCartTotals.Subtotal)
                },
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Tax"),
                    Value = String.Format("$ {0:#,0.00}", shoppingCartTotals.TotalTax)
                },
                new Total()
                {
                    Title = resources.GetResourceString("Kadena.Checkout.Totals.Totals"),
                    Value = String.Format("$ {0:#,0.00}", shoppingCartTotals.TotalPrice)
                }
            }.ToList();
        }

        private void CheckCurrentOrDefaultAddress(CheckoutPage page)
        {
            if (page.DeliveryAddresses.items.Count == 0)
            {
                return;
            }

            var currentAddressId = kenticoProvider.GetCurrentCartAddresId();
            if (currentAddressId != 0 && page.DeliveryAddresses.items.Any(a => a.Id == currentAddressId))
            {
                page.DeliveryAddresses.CheckAddress(currentAddressId);
            }
            else
            {
                var defaultAddressId = kenticoUsers.GetCurrentCustomer().DefaultShippingAddressId;
                if (defaultAddressId == 0)
                {
                    defaultAddressId = page.DeliveryAddresses.GetDefaultAddressId();
                }

                kenticoProvider.SetShoppingCartAddress(defaultAddressId);
                page.DeliveryAddresses.CheckAddress(defaultAddressId);
            }
        }

        private string GetUserNotificationString()
        {
            var userNotification = string.Empty;
            var userNotificationLocalizationKey = kenticoProvider.GetCurrentSiteCodeName() + ".Kadena.Settings.Address.NotificationMessage";
            if (!kenticoProvider.IsCurrentCultureDefault())
            {
                userNotification = resources.GetResourceString(userNotificationLocalizationKey) == userNotificationLocalizationKey ? string.Empty : resources.GetResourceString(userNotificationLocalizationKey);
            }
            return userNotification;
        }

        private void CheckCurrentOrDefaultShipping(DeliveryCarriers deliveryMethods)
        {
            int currentShipping = kenticoProvider.GetCurrentCartShippingOptionId();

            if (deliveryMethods.IsPresent(currentShipping) && !deliveryMethods.IsDisabled(currentShipping))
            {
                deliveryMethods.CheckMethod(currentShipping);
            }
            else
            {
                SetDefaultShipping(deliveryMethods);
            }
        }

        private void SetDefaultShipping(DeliveryCarriers deliveryMethods)
        {
            int defaultMethodId = deliveryMethods.GetDefaultMethodId();
            kenticoProvider.SelectShipping(defaultMethodId);
            deliveryMethods.CheckMethod(defaultMethodId);
        }

        private void UnsetShipping()
        {
            kenticoProvider.SelectShipping(0);
        }

        private void SetPricesVisibility(CheckoutPage page)
        {
            if (!kenticoUsers.UserCanSeePrices())
            {
                page.Products.HidePrices();
            }
        }

        private void SetPricesVisibility(CheckoutPageDeliveryTotals page)
        {
            if (!kenticoUsers.UserCanSeePrices())
            {
                page.DeliveryMethods.HidePrices();
            }
        }
       
        public CheckoutPage SelectShipipng(int id)
        {
            kenticoProvider.SelectShipping(id);
            return GetCheckoutPage();
        }

        public CheckoutPage SelectAddress(int id)
        {
            kenticoProvider.SetShoppingCartAddress(id);
            var checkoutPage = GetCheckoutPage();
            checkoutPage.DeliveryAddresses.CheckAddress(id);
            return checkoutPage;
        }

        public CheckoutPage ChangeItemQuantity(int id, int quantity)
        {
            kenticoProvider.SetCartItemQuantity(id, quantity);
            return GetCheckoutPage();
        }

        public CheckoutPage RemoveItem(int id)
        {
            kenticoProvider.RemoveCartItem(id);
            var itemsCount = kenticoProvider.GetShoppingCartItemsCount();
            if (itemsCount == 0)
            {
                kenticoProvider.ClearCart();
            }

            return GetCheckoutPage();
        }

        public CartItemsPreview ItemsPreview()
        {
            bool userCanSeePrices = kenticoUsers.UserCanSeePrices();
            var cartItems = kenticoProvider.GetShoppingCartItems(userCanSeePrices);

            var preview = new CartItemsPreview
            {
                EmptyCartMessage = resources.GetResourceString("Kadena.Checkout.CartIsEmpty"),
                SummaryPrice = new CartPrice(),

                Items = cartItems.ToList()
            };

            if (userCanSeePrices)
            {
                var cartItemsTotals = kenticoProvider.GetShoppingCartTotals();
                preview.SummaryPrice = new CartPrice()
                {
                    PricePrefix = resources.GetResourceString("Kadena.Checkout.ItemPricePrefix"),
                    Price = string.Format("{0:#,0.00}", cartItemsTotals.TotalItemsPrice)
                };
            }

            return preview;
        }

        public async Task<AddToCartResult> AddToCart(NewCartItem item)
        {
            var mailingList = await mailingService.GetMailingList(item.ContainerId);
            var addedItem = kenticoProvider.AddCartItem(item, mailingList);
            var result = new AddToCartResult
            {
                CartPreview = ItemsPreview(),
                Confirmation = new RequestResult
                {
                    AlertMessage = resources.GetResourceString("Kadena.Product.ItemsAddedToCart")
                }
            };
            return result;
        }

        bool GetOtherAddressSettingsValue()
        {
            var settingsKey = resources.GetSettingsKey("KDA_AllowCustomShippingAddress");
            bool otherAddressAvailable = false;
            bool.TryParse(settingsKey, out otherAddressAvailable);
            return otherAddressAvailable;
        }
    }
}
