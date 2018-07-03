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
using Kadena.Models.SiteSettings;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Models.ShoppingCarts;

namespace Kadena.BusinessLogic.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IKenticoSiteProvider kenticoSite;
        private readonly IKenticoLocalizationProvider localization;
        private readonly IKenticoPermissionsProvider permissions;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoCustomerProvider kenticoCustomer;
        private readonly IKenticoAddressBookProvider kenticoAddresses;
        private readonly IKenticoResourceService resources;
        private readonly ITaxEstimationService taxCalculator;
        private readonly IKListService mailingService;
        private readonly IUserDataServiceClient userDataClient;
        private readonly IShoppingCartProvider shoppingCart;
        private readonly IShoppingCartItemsProvider shoppingCartItems;
        private readonly ICheckoutPageFactory checkoutfactory;
        private readonly IKenticoLogger log;
        private readonly IProductsService productsService;
        private readonly IImageService imageService;
        private readonly IKenticoSkuProvider skus;
        private readonly IOrderItemCheckerService orderChecker;

        public ShoppingCartService(IKenticoSiteProvider kenticoSite,
                                   IKenticoLocalizationProvider localization,
                                   IKenticoPermissionsProvider permissions,
                                   IKenticoUserProvider kenticoUsers,
                                   IKenticoCustomerProvider kenticoCustomer,
                                   IKenticoAddressBookProvider addresses,
                                   IKenticoResourceService resources,
                                   ITaxEstimationService taxCalculator,
                                   IKListService mailingService,
                                   IUserDataServiceClient userDataClient,
                                   IShoppingCartProvider shoppingCart,
                                   IShoppingCartItemsProvider shoppingCartItems,
                                   ICheckoutPageFactory checkoutfactory,
                                   IKenticoLogger log,
                                   IProductsService productsService,
                                   IImageService imageService,
                                   IKenticoSkuProvider skus,
                                   IOrderItemCheckerService orderChecker)
        {
            this.kenticoSite = kenticoSite ?? throw new ArgumentNullException(nameof(kenticoSite));
            this.localization = localization ?? throw new ArgumentNullException(nameof(localization));
            this.permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            this.kenticoUsers = kenticoUsers ?? throw new ArgumentNullException(nameof(kenticoUsers));
            this.kenticoCustomer= kenticoCustomer ?? throw new ArgumentNullException(nameof(kenticoCustomer));
            this.kenticoAddresses = addresses ?? throw new ArgumentNullException(nameof(addresses));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.taxCalculator = taxCalculator ?? throw new ArgumentNullException(nameof(taxCalculator));
            this.mailingService = mailingService ?? throw new ArgumentNullException(nameof(mailingService));
            this.userDataClient = userDataClient ?? throw new ArgumentNullException(nameof(userDataClient));
            this.shoppingCart = shoppingCart ?? throw new ArgumentNullException(nameof(shoppingCart));
            this.shoppingCartItems = shoppingCartItems ?? throw new ArgumentNullException(nameof(shoppingCartItems));
            this.checkoutfactory = checkoutfactory ?? throw new ArgumentNullException(nameof(checkoutfactory));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));
            this.imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            this.skus = skus ?? throw new ArgumentNullException(nameof(skus));
            this.orderChecker = orderChecker ?? throw new ArgumentNullException(nameof(orderChecker));
        }

        public async Task<CheckoutPage> GetCheckoutPage()
        {
            var addresses = kenticoAddresses.GetCustomerAddresses(AddressType.Shipping);
            var paymentMethods = shoppingCart.GetPaymentMethods();
            var emailConfirmationEnabled = resources.GetSettingsKey<bool>(Settings.KDA_UseNotificationEmailsOnCheckout);
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

            if (creditCardMethod != null && resources.GetSiteSettingsKey<bool>(Settings.KDA_CreditCard_EnableSaveCard))
            {
                var storedCardsResult = await userDataClient.GetValidCardTokens(userId);

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

            var isShippingApplicable = shoppingCartItems.GetCheckoutCartItems()
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

        public int SaveTemporaryAddress(DeliveryAddress deliveryAddress)
        {
            return shoppingCart.SetTemporaryShoppingCartAddress(deliveryAddress);
            
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
            if (deliveryAddress != null)
            {
                var shippingCost = shoppingCart.GetCurrentCartShippingCost();
                shoppingCartTotals.TotalTax = await taxCalculator.EstimateTax(deliveryAddress, (double)shoppingCartTotals.TotalItemsPrice, shippingCost);
            }
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
                var defaultAddressId = kenticoCustomer.GetCurrentCustomer().DefaultShippingAddressId;
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
            var customerAddresses = kenticoAddresses.GetCustomerAddresses(AddressType.Shipping);
            var userNotificationString = GetUserNotificationString();
            var otherAddressEnabled = GetOtherAddressSettingsValue();

            var addresses = checkoutfactory.CreateDeliveryAddresses(customerAddresses.ToList(), userNotificationString, otherAddressEnabled);

            addresses.CheckAddress(checkedAddressId);

            return addresses;
        }

        public CartItems ChangeItemQuantity(int id, int quantity)
        {
            var item = shoppingCartItems.GetCartItemEntity(id);

            if (item == null)
            {
                throw new ArgumentOutOfRangeException(string.Format( 
                    resources.GetResourceString("Kadena.Product.ItemInCartNotFound"),id));
            }

            if (quantity < 1)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    resources.GetResourceString("Kadena.Product.NegativeQuantityError"), quantity));
            }

            if (!item.ProductType.Contains(ProductTypes.InventoryProduct) && !item.ProductType.Contains(ProductTypes.POD) && !item.ProductType.Contains(ProductTypes.StaticProduct))
            {
                throw new Exception(resources.GetResourceString("Kadena.Product.QuantityForTypeError"));
            }

            var itemSku = skus.GetSKU(item.SKUID);

            if (itemSku == null)
            {
                throw new Exception($"SKU with SKUID {item.SKUID} not found");
            }

            if (item.ProductType.Contains(ProductTypes.InventoryProduct) && itemSku.SellOnlyIfAvailable && quantity > itemSku.AvailableItems)
            {
                throw new Exception(string.Format(
                    resources.GetResourceString("Kadena.Product.SetQuantityForItemError"), quantity, item.CartItemText));
            }

            orderChecker.CheckMinMaxQuantity(itemSku, quantity);

            item.SKUUnits = quantity;

            var price = productsService.GetPriceByCustomModel(item.ProductPageID, item.SKUUnits);

            if (price > decimal.MinusOne)
            {
                item.CartItemPrice = price;
            }

            shoppingCartItems.SetCartItemQuantity(item, quantity);

            return GetCartItems();
        }

        public CartItems RemoveItem(int id)
        {
            shoppingCartItems.RemoveCartItem(id);
            var itemsCount = shoppingCartItems.GetShoppingCartItemsCount();
            if (itemsCount == 0)
            {
                shoppingCart.ClearCart();
            }

            return GetCartItems();
        }

        public CartItems GetCartItems()
        {
            var cartItems = shoppingCartItems.GetCheckoutCartItems().ToList();
            var cartItemsTotals = shoppingCart.GetShoppingCartTotals();
            var countOfItemsString = cartItems.Count == 1 ? resources.GetResourceString("Kadena.Checkout.ItemSingular") : resources.GetResourceString("Kadena.Checkout.ItemPlural");
            cartItems.ForEach(i => i.Image = imageService.GetThumbnailLink(i.Image));

            cartItems.ForEach(i => 
                {
                    i.Delivery = string.Empty;

                    if (i.IsMailingList && i.IsTemplated)
                    {
                        var delivery = resources.GetResourceString("Kadena.Checkout.MailingDelivery");
                        i.Delivery = string.Format(delivery, i.Quantity);
                    }
                }
             );
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
            var cartItems = shoppingCartItems.GetCheckoutCartItems(userCanSeePrices).ToList();
            cartItems.ForEach(i => i.Image = imageService.GetThumbnailLink(i.Image));

            var preview = new CartItemsPreview
            {
                EmptyCartMessage = resources.GetResourceString("Kadena.Checkout.CartIsEmpty"),
                SummaryPrice = new CartPrice(),

                Items = cartItems
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



        public async Task<AddToCartResult> AddToCart(NewCartItem newItem)
        {
            var addedAmount = newItem.Quantity;

            if (addedAmount < 1)
            {
                throw new ArgumentException(resources.GetResourceString("Kadena.Product.InsertedAmmountValueIsNotValid"));
            }

            var cartItem = shoppingCartItems.GetOrCreateCartItem(newItem);

            var sku = skus.GetSKU(cartItem.SKUID) ?? throw new ArgumentException($"Unable to find SKU {cartItem.SKUID}");

            if (ProductTypes.IsOfType(cartItem.ProductType, ProductTypes.InventoryProduct))
            {
                orderChecker.EnsureInventoryAmount(sku, newItem.Quantity, cartItem.SKUUnits);
            }
            
            if (ProductTypes.IsOfType(cartItem.ProductType, ProductTypes.MailingProduct))
            {
                await SetMailingList(cartItem, newItem.ContainerId, addedAmount);
            }

            shoppingCartItems.SetArtwork(cartItem, newItem.DocumentId);

            // do this before calculating dynamic price
            if (ProductTypes.IsOfType(cartItem.ProductType, ProductTypes.TemplatedProduct))
            {
                orderChecker.CheckMinMaxQuantity(skus.GetSKU(cartItem.SKUID),
                                    newItem.Quantity);
                cartItem.SKUUnits = newItem.Quantity;
            }
            else if(!ProductTypes.IsOfType(cartItem.ProductType, ProductTypes.MailingProduct))
            {
                var totalQuantity = cartItem.SKUUnits + newItem.Quantity;
                orderChecker.CheckMinMaxQuantity(skus.GetSKU(cartItem.SKUID),
                                    totalQuantity);
                cartItem.SKUUnits = totalQuantity;
            }
            
            var price = productsService.GetPriceByCustomModel(newItem.DocumentId, cartItem.SKUUnits);
            if (price != decimal.MinusOne)
            {
                cartItem.CartItemPrice = price;
            }

            if (!string.IsNullOrEmpty(newItem.CustomProductName))
            {
                cartItem.CartItemText = newItem.CustomProductName;
            }

            shoppingCartItems.SaveCartItem(cartItem);

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
       
        private async Task SetMailingList(CartItemEntity cartItem, Guid containerId, int addedAmount)
        {
            var mailingList = await mailingService.GetMailingList(containerId);

            if (mailingList?.AddressCount != addedAmount)
            {
                throw new ArgumentException(resources.GetResourceString("Kadena.Product.InsertedAmmountValueIsNotValid"));
            }

            if (mailingList != null)
            {
                cartItem.MailingListName = mailingList.Name;
                cartItem.MailingListGuid = Guid.Parse(mailingList.Id);
            }

            cartItem.SKUUnits = addedAmount;
        }

        

        private bool GetOtherAddressSettingsValue()
        {
            var settingsKey = resources.GetSiteSettingsKey(Settings.KDA_AllowCustomShippingAddress);
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
