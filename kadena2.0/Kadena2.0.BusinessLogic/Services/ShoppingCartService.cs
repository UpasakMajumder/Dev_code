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
using Kadena.Models.AddToCart;

namespace Kadena.BusinessLogic.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IKenticoSiteProvider kenticoSite;
        private readonly IKenticoLocalizationProvider localization;
        private readonly IKenticoPermissionsProvider permissions;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoAddressBookProvider kenticoAddresses;
        private readonly IKenticoResourceService resources;
        private readonly ITaxEstimationService taxCalculator;
        private readonly IKListService mailingService;
        private readonly IUserDataServiceClient userDataClient;
        private readonly IShoppingCartProvider shoppingCart;
        private readonly IShoppingCartItemsProvider shoppingCartItems;
        private readonly ICheckoutPageFactory checkoutfactory;
        private readonly IKenticoLogger log;
        private readonly IKenticoAddressBookProvider addressBookProvider;
        private readonly IKenticoProductsProvider productsProvider;
        private readonly IKenticoBusinessUnitsProvider businessUnitsProvider;
        private readonly IDynamicPriceRangeProvider dynamicPrices;

        public ShoppingCartService(IKenticoSiteProvider kenticoSite,
                                   IKenticoLocalizationProvider localization,
                                   IKenticoPermissionsProvider permissions,
                                   IKenticoUserProvider kenticoUsers,
                                   IKenticoAddressBookProvider addresses,
                                   IKenticoResourceService resources,
                                   ITaxEstimationService taxCalculator,
                                   IKListService mailingService,
                                   IUserDataServiceClient userDataClient,
                                   IShoppingCartProvider shoppingCart,
                                   IShoppingCartItemsProvider shoppingCartItems,
                                   ICheckoutPageFactory checkoutfactory,
                                   IKenticoLogger log,
                                   IKenticoAddressBookProvider addressBookProvider,
                                   IKenticoProductsProvider productsProvider,
                                   IKenticoBusinessUnitsProvider businessUnitsProvider,
                                   IDynamicPriceRangeProvider dynamicPrices)
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
            if (addresses == null)
            {
                throw new ArgumentNullException(nameof(addresses));
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
            if (shoppingCartItems == null)
            {
                throw new ArgumentNullException(nameof(shoppingCartItems));
            }
            if (checkoutfactory == null)
            {
                throw new ArgumentNullException(nameof(checkoutfactory));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }
            if (addressBookProvider == null)
            {
                throw new ArgumentNullException(nameof(addressBookProvider));
            }
            if (productsProvider == null)
            {
                throw new ArgumentNullException(nameof(productsProvider));
            }
            if (businessUnitsProvider == null)
            {
                throw new ArgumentNullException(nameof(businessUnitsProvider));
            }
            if (dynamicPrices == null)
            {
                throw new ArgumentNullException(nameof(dynamicPrices));
            }

            this.kenticoSite = kenticoSite;
            this.localization = localization;
            this.permissions = permissions;
            this.kenticoUsers = kenticoUsers;
            this.kenticoAddresses = addresses;
            this.resources = resources;
            this.taxCalculator = taxCalculator;
            this.mailingService = mailingService;
            this.userDataClient = userDataClient;
            this.shoppingCart = shoppingCart;
            this.shoppingCartItems = shoppingCartItems;
            this.checkoutfactory = checkoutfactory;
            this.log = log;
            this.addressBookProvider = addressBookProvider;
            this.productsProvider = productsProvider;
            this.businessUnitsProvider = businessUnitsProvider;
            this.dynamicPrices = dynamicPrices;
        }

        public async Task<CheckoutPage> GetCheckoutPage()
        {
            var addresses = kenticoAddresses.GetCustomerAddresses(AddressType.Shipping);
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

            if (creditCardMethod != null && resources.GetSettingsKey("KDA_CreditCard_EnableSaveCard").ToLower() == "true")
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

            var isShippingApplicable = shoppingCartItems.GetShoppingCartItems()
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
                shoppingCartTotals.TotalTax = await taxCalculator.EstimateTotalTax(deliveryAddress);
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
            var customerAddresses = kenticoAddresses.GetCustomerAddresses(AddressType.Shipping);
            var userNotificationString = GetUserNotificationString();
            var otherAddressEnabled = GetOtherAddressSettingsValue();

            var addresses = checkoutfactory.CreateDeliveryAddresses(customerAddresses.ToList(), userNotificationString, otherAddressEnabled);

            addresses.CheckAddress(checkedAddressId);

            return addresses;
        }

        public CartItems ChangeItemQuantity(int id, int quantity)
        {
            shoppingCartItems.SetCartItemQuantity(id, quantity);
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
            var cartItems = shoppingCartItems.GetShoppingCartItems();
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
            var cartItems = shoppingCartItems.GetShoppingCartItems(userCanSeePrices);

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

        public async Task<AddToCartResult> AddToCart(NewCartItem newItem)
        {
            var addedAmount = newItem.Quantity;

            if (addedAmount < 1)
            {
                throw new ArgumentException(resources.GetResourceString("Kadena.Product.InsertedAmmountValueIsNotValid"));
            }

            var cartItem = shoppingCartItems.GetOrCreateCartItem(newItem);

            if (ProductTypes.IsOfType(cartItem.ProductType, ProductTypes.InventoryProduct))
            {
                EnsureInventoryAmount(cartItem, newItem.Quantity, cartItem.SKUUnits);
            }
            
            if (ProductTypes.IsOfType(cartItem.ProductType, ProductTypes.MailingProduct))
            {
                await SetMailingList(cartItem, newItem.ContainerId, addedAmount);
            }

            shoppingCartItems.SetArtwork(cartItem, newItem.DocumentId);

            SetDynamicPrice(cartItem, newItem.DocumentId);

            if (!string.IsNullOrEmpty(newItem.CustomProductName))
            {
                cartItem.CartItemText = newItem.CustomProductName;
            }

            if (ProductTypes.IsOfType(cartItem.ProductType, ProductTypes.TemplatedProduct))
            {
                cartItem.SKUUnits = newItem.Quantity;
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

        private void SetDynamicPrice(CartItemEntity cartItem, int documentId)
        {
            var price = dynamicPrices.GetDynamicPrice(cartItem.SKUUnits, documentId);
            if (price > decimal.MinusOne)
            {
                cartItem.CartItemPrice = price;
            }
            else
            {
                cartItem.CartItemPrice = null;
            }
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

        private void EnsureInventoryAmount(CartItemEntity item, int addedQuantity, int resultedQuantity)
        {
            var availableQuantity = shoppingCart.GetStockQuantity(item);

            if (addedQuantity > availableQuantity)
            {
                throw new ArgumentException(resources.GetResourceString("Kadena.Product.LowerNumberOfAvailableProducts"));
            }
            else
            {
                if (resultedQuantity > availableQuantity)
                {
                    throw new ArgumentException(string.Format(resources.GetResourceString("Kadena.Product.ItemsInCartExceeded"),
                        resultedQuantity - addedQuantity, availableQuantity - resultedQuantity + addedQuantity));
                }
            }
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

        public DistributorCart GetCartDistributorData(int skuID, int inventoryType = 1)
        {
            int businessUnitsCount = businessUnitsProvider.GetUserBusinessUnits(kenticoUsers.GetCurrentUser().UserId)?.Count ?? 0;
            if (businessUnitsCount == 0)
            {
                throw new Exception(resources.GetResourceString("Kadena.AddToCart.BusinessUnitError"));
            }
            if (inventoryType == 1 && !productsProvider.ProductHasValidSKUNumber(skuID))
            {
                throw new Exception(resources.GetResourceString("KDA.Cart.InvalidProduct"));
            }
            int availableQty = inventoryType == 1 ? productsProvider.GetSkuAvailableQty(skuID) : -1;
            if (availableQty == 0)
            {
                throw new Exception(resources.GetResourceString("Kadena.AddToCart.NoStockAvailableError"));
            }
            int allocatedQty = inventoryType == 1 ? shoppingCart.GetAllocatedQuantity(skuID, kenticoUsers.GetCurrentUser().UserId) : -1;
            if (allocatedQty == 0)
            {
                throw new Exception(resources.GetResourceString("KDA.Cart.Update.ProductNotAllocatedMessage"));
            }
            return new DistributorCart()
            {
                SKUID = skuID,
                CartType = inventoryType,
                AvailableQuantity = availableQty,
                AllocatedQuantity = allocatedQty,
                Items = GetDistributorCartItems(skuID, inventoryType)
            };
        }

        private List<DistributorCartItem> GetDistributorCartItems(int skuID, int inventoryType = 1)
        {
            CampaignsProduct product = productsProvider.GetCampaignProduct(skuID);
            if (product == null)
            {
                throw new Exception("Invalid product");
            }
            List<AddressData> distributors = addressBookProvider.GetAddressesListByUserID(kenticoUsers.GetCurrentUser().UserId, inventoryType, product.CampaignID);
            return distributors.Select(x =>
            {
                return new DistributorCartItem()
                {
                    DistributorID = x.AddressID,
                    ShoppingCartID = x.DistributorShoppingCartID,
                    Quantity = shoppingCart.GetItemQuantity(skuID, x.DistributorShoppingCartID)
                };
            }).ToList();
        }

        public int UpdateDistributorCarts(DistributorCart cartDistributorData)
        {
            if (cartDistributorData == null || (cartDistributorData != null && cartDistributorData.Items.Count <= 0))
            {
                throw new Exception("Invalid request");
            }
            CampaignsProduct product = productsProvider.GetCampaignProduct(cartDistributorData.SKUID);
            if (product == null)
            {
                throw new Exception("Invalid product");
            }
            cartDistributorData.Items.Where(i => i.ShoppingCartID.Equals(default(int)) && i.Quantity > 0)
                                    ?.ToList().ForEach(x => CreateDistributorCart(x, product, cartDistributorData.CartType));
            cartDistributorData.Items.Where(i => i.ShoppingCartID > 0 && i.Quantity > 0)
                                    ?.ToList().ForEach(x => shoppingCart.UpdateDistributorCart(x, product, cartDistributorData.CartType));
            cartDistributorData.Items.Where(i => i.ShoppingCartID > 0 && i.Quantity == 0)
                                    ?.ToList().ForEach(x => shoppingCart.DeleteDistributorCartItem(x.ShoppingCartID, cartDistributorData.SKUID));
            return shoppingCart.GetDistributorCartCount(kenticoUsers.GetCurrentUser().UserId, product.CampaignID, cartDistributorData.CartType);
        }

        private void CreateDistributorCart(DistributorCartItem distributorCartItem, CampaignsProduct product, int inventoryType = 1)
        {
            int cartID = shoppingCart.CreateDistributorCart(distributorCartItem, product, kenticoUsers.GetCurrentUser().UserId, inventoryType);
            shoppingCart.AddDistributorCartItem(cartID, distributorCartItem, product, inventoryType);
        }
    }
}
