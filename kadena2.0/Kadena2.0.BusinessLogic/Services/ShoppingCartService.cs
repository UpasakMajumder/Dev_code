using System;
using System.Linq;
using System.Threading.Tasks;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Checkout;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Product;
using Kadena.BusinessLogic.Factories.Checkout;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.CreditCard;
using Kadena2.MicroserviceClients.Contracts;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IKenticoSiteProvider kenticoSite;
        private readonly IKenticoLocalizationProvider localization;
        private readonly IKenticoPermissionsProvider permissions;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoResourceService resources;
        private readonly ITaxEstimationService taxCalculator;
        private readonly IKListService mailingService;
        private readonly IUserDataServiceClient userDataClient;
        private readonly IShoppingCartProvider shoppingCart;
        private readonly ICheckoutPageFactory checkoutfactory;
        private readonly IKenticoLogger log;

        public ShoppingCartService(IKenticoSiteProvider kenticoSite,
                                   IKenticoLocalizationProvider localization,
                                   IKenticoPermissionsProvider permissions,
                                   IKenticoUserProvider kenticoUsers,
                                   IKenticoResourceService resources,
                                   ITaxEstimationService taxCalculator,
                                   IKListService mailingService,
                                   IUserDataServiceClient userDataClient,
                                   IShoppingCartProvider shoppingCart,
                                   ICheckoutPageFactory checkoutfactory,
                                   IKenticoLogger log)
        {
            if (kenticoSite == null)
            {
                throw new ArgumentNullException(nameof(kenticoSite));
            }
            if (localization == null)
            {
                throw new ArgumentNullException(nameof(localization));
            }
            if (permissions == null)
            {
                throw new ArgumentNullException(nameof(permissions));
            }
            if (kenticoUsers == null)
            {
                throw new ArgumentNullException(nameof(kenticoUsers));
            }
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (taxCalculator == null)
            {
                throw new ArgumentNullException(nameof(taxCalculator));
            }
            if (mailingService == null)
            {
                throw new ArgumentNullException(nameof(mailingService));
            }
            if (userDataClient == null)
            {
                throw new ArgumentNullException(nameof(userDataClient));
            }
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }
            if (checkoutfactory == null)
            {
                throw new ArgumentNullException(nameof(checkoutfactory));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            this.kenticoSite = kenticoSite;
            this.localization = localization;
            this.permissions = permissions;
            this.kenticoUsers = kenticoUsers;
            this.resources = resources;
            this.taxCalculator = taxCalculator;
            this.mailingService = mailingService;
            this.userDataClient = userDataClient;
            this.shoppingCart = shoppingCart;
            this.checkoutfactory = checkoutfactory;
            this.log = log;
        }

        public async Task<CheckoutPage> GetCheckoutPage()
        {
            var addresses = kenticoUsers.GetCustomerAddresses(AddressType.Shipping);
            var paymentMethods = shoppingCart.GetPaymentMethods();
            var emailConfirmationEnabled = resources.GetSettingsKey<bool>("KDA_UseNotificationEmailsOnCheckout");
            var currentUserId = kenticoUsers.GetCurrentUser().UserId;

            var checkoutPage = new CheckoutPage()
            {
                EmptyCart = checkoutfactory.CreateCartEmptyInfo(),
                Products = GetCartItems(),
                DeliveryAddresses = GetDeliveryAddresses(),
                PaymentMethods = checkoutfactory.CreatePaymentMethods(paymentMethods),
                Submit = checkoutfactory.CreateSubmitButton(),
                ValidationMessage = resources.GetResourceString("Kadena.Checkout.ValidationError"),
                EmailConfirmation = checkoutfactory.CreateNotificationEmail(emailConfirmationEnabled)
            };

            CheckCurrentOrDefaultAddress(checkoutPage);
            await ArragnePaymentMethods(checkoutPage.PaymentMethods, currentUserId);
            checkoutPage.SetDisplayType();
            return checkoutPage;
        }

        private async Task ArragnePaymentMethods(PaymentMethods methods, int userId)
        {
            var creditCardMethod = methods.Items.FirstOrDefault(pm => pm.ClassName == "KDA.PaymentMethods.CreditCard");

            if ( creditCardMethod != null && resources.GetSettingsKey("KDA_CreditCard_EnableSaveCard").ToLower() == "true")
            {
                var storedCardsResult = await userDataClient.GetCardTokens(userId);

                if (storedCardsResult.Success)
                {
                    creditCardMethod.Items = storedCardsResult.Payload.Select(c => 
                        new StoredCard
                        {
                            Id = c.Id,
                            Label = c.Name
                        }
                    ).ToList();
                }
                else
                {
                    creditCardMethod.Items = new List<StoredCard>();
                    log.LogError("GetStroredCards", storedCardsResult.ErrorMessages);
                }

                creditCardMethod.Items.Add(new StoredCard { Checked = true, Id = string.Empty, Label = resources.GetResourceString("Kadena.CreditCard.EnterNew") });
            }

            methods.CheckDefault();
            methods.CheckPayability();
        }

        public async Task<CheckoutPageDeliveryTotals> GetDeliveryAndTotals()
        {
            var deliveryAddress = shoppingCart.GetCurrentCartShippingAddress();

            var isShippingApplicable = shoppingCart.GetShoppingCartItems()
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

            if (permissions.UserCanSeePrices())
            {
                await UpdateTotals(result, deliveryAddress);
            }
            else
            {
                result.DeliveryMethods?.HidePrices();
            }
            
            return result;
        }

        public async Task<CheckoutPageDeliveryTotals> SetDeliveryAddress(DeliveryAddress deliveryAddress)
        {
            shoppingCart.SetShoppingCartAddress(deliveryAddress);
            return await GetDeliveryAndTotals();
        }

        private DeliveryCarriers GetDeliveryMethods(bool isShippingApplicable)
        {
            if (!isShippingApplicable)
            {
                return new DeliveryCarriers();
            }

            var carriers = shoppingCart.GetShippingCarriers();
            var deliveryMethods = new DeliveryCarriers()
            {
                Title = resources.GetResourceString("Kadena.Checkout.Delivery.Title"),
                Description = resources.GetResourceString("Kadena.Checkout.DeliveryMethodDescription"),
                Items = carriers.ToList()
            };

            deliveryMethods.RemoveCarriersWithoutOptions();

            int currentShipping = shoppingCart.GetCurrentCartShippingOptionId();
            int checkedShipping = deliveryMethods.CheckCurrentOrDefaultShipping(currentShipping);

            if (currentShipping != checkedShipping)
            {
                shoppingCart.SelectShipping(checkedShipping);
            }

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
            var shoppingCartTotals = shoppingCart.GetShoppingCartTotals();
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
            if ((page?.DeliveryAddresses?.items?.Count ?? 0) == 0)
            {
                return;
            }

            var currentAddressId = shoppingCart.GetCurrentCartShippingAddress()?.Id ?? 0;
            if (currentAddressId != 0 && page.DeliveryAddresses.items.Any(a => a.Id == currentAddressId))
            {
                page.DeliveryAddresses.CheckAddress(currentAddressId);
            }
            else
            {
                var defaultAddressId = kenticoUsers.GetCurrentCustomer().DefaultShippingAddressId;
                if (defaultAddressId == 0 || shoppingCart.GetAddress(defaultAddressId) == null)
                {
                    defaultAddressId = page.DeliveryAddresses.GetDefaultAddressId();
                }

                shoppingCart.SetShoppingCartAddress(defaultAddressId);
                page.DeliveryAddresses.CheckAddress(defaultAddressId);
            }
        }

        private string GetUserNotificationString()
        {
            var userNotification = string.Empty;
            var userNotificationLocalizationKey = kenticoSite.GetCurrentSiteCodeName() + ".Kadena.Settings.Address.NotificationMessage";
            if (!localization.IsCurrentCultureDefault())
            {
                userNotification = resources.GetResourceString(userNotificationLocalizationKey) == userNotificationLocalizationKey ? string.Empty : resources.GetResourceString(userNotificationLocalizationKey);
            }
            return userNotification;
        }

        private void UnsetShipping()
        {
            shoppingCart.SelectShipping(0);
        }

        public async Task<CheckoutPageDeliveryTotals> SelectShipping(int id)
        {
            shoppingCart.SelectShipping(id);
            return await GetDeliveryAndTotals();
        }

        public DeliveryAddresses SelectAddress(int id)
        {
            shoppingCart.SetShoppingCartAddress(id);
            return GetDeliveryAddresses(id);
        }

        private DeliveryAddresses GetDeliveryAddresses(int checkedAddressId = 0)
        {
            var customerAddresses = kenticoUsers.GetCustomerAddresses(AddressType.Shipping);
            var userNotificationString = GetUserNotificationString();
            var otherAddressEnabled = GetOtherAddressSettingsValue();

            var addresses = checkoutfactory.CreateDeliveryAddresses(customerAddresses.ToList(), userNotificationString, otherAddressEnabled);

            addresses.CheckAddress(checkedAddressId);

            return addresses;
        }

        public CartItems ChangeItemQuantity(int id, int quantity)
        {
            shoppingCart.SetCartItemQuantity(id, quantity);
            return GetCartItems();
        }

        public CartItems RemoveItem(int id)
        {
            shoppingCart.RemoveCartItem(id);
            var itemsCount = shoppingCart.GetShoppingCartItemsCount();
            if (itemsCount == 0)
            {
                shoppingCart.ClearCart();
            }

            return GetCartItems();
        }

        public CartItems GetCartItems()
        {
            var cartItems = shoppingCart.GetShoppingCartItems();
            var cartItemsTotals = shoppingCart.GetShoppingCartTotals();
            var countOfItemsString = cartItems.Length == 1 ? resources.GetResourceString("Kadena.Checkout.ItemSingular") : resources.GetResourceString("Kadena.Checkout.ItemPlural");

            var products = checkoutfactory.CreateProducts(cartItems, cartItemsTotals, countOfItemsString);

            if (!permissions.UserCanSeePrices())
            {
                products.HidePrices();
            }

            return products;
        }

        public CartItemsPreview ItemsPreview()
        {
            bool userCanSeePrices = permissions.UserCanSeePrices();
            var cartItems = shoppingCart.GetShoppingCartItems(userCanSeePrices);

            var preview = new CartItemsPreview
            {
                EmptyCartMessage = resources.GetResourceString("Kadena.Checkout.CartIsEmpty"),
                SummaryPrice = new CartPrice(),

                Items = cartItems.ToList()
            };

            if (userCanSeePrices)
            {
                var cartItemsTotals = shoppingCart.GetShoppingCartTotals();
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
            var addedItem = shoppingCart.AddCartItem(item, mailingList);
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

        private bool GetOtherAddressSettingsValue()
        {
            var settingsKey = resources.GetSettingsKey("KDA_AllowCustomShippingAddress");
            bool otherAddressAvailable = false;
            bool.TryParse(settingsKey, out otherAddressAvailable);
            return otherAddressAvailable;
        }

        public List<int> GetLoggedInUserCartData(int inventoryType, int userID, int campaignID = 0)
        {
            return shoppingCart.GetShoppingCartIDByInventoryType(inventoryType, userID, campaignID);
        }
    }
}
