using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Checkout;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Product;
using Kadena.BusinessLogic.Factories.Checkout;
using Kadena.Models.Settings;

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
            var items = cartItems.Length == 1 ? resources.GetResourceString("Kadena.Checkout.ItemSingular") : resources.GetResourceString("Kadena.Checkout.ItemPlural");
            var count = cartItems?.Length ?? 0;

            var otherAddressSettingValue = resources.GetSettingsKey("KDA_AllowCustomShippingAddress");

            var userNotification = string.Empty;
            var userNotificationLocalizationKey = kenticoProvider.GetCurrentSiteCodeName() + ".Kadena.Settings.Address.NotificationMessage";
            if (!kenticoProvider.IsCurrentCultureDefault())
            {
                userNotification = resources.GetResourceString(userNotificationLocalizationKey) == userNotificationLocalizationKey ? string.Empty : resources.GetResourceString(userNotificationLocalizationKey);
            }
            bool otherAddressAvailable = false;
            bool.TryParse(otherAddressSettingValue, out otherAddressAvailable);

            var checkoutPage = new CheckoutPage()
            {
                EmptyCart = checkoutfactory.CreateCartEmptyInfo(cartItems),
                Products = new CartItems()
                {
                    Number = string.Format(resources.GetResourceString("Kadena.Checkout.CountOfItems"), count, items),
                    Items = cartItems.ToList(),
                    ButtonLabels = new ButtonLabels
                    {
                        Edit = resources.GetResourceString("Kadena.Checkout.EditButton"),
                        Remove = resources.GetResourceString("Kadena.Checkout.RemoveButton"),
                    },
                    SummaryPrice = new CartPrice
                    {
                        PricePrefix = resources.GetResourceString("Kadena.Checkout.ItemPricePrefix"),
                        Price = string.Format("{0:#,0.00}", cartItemsTotals.TotalItemsPrice)
                    }
                },
                DeliveryAddresses = new DeliveryAddresses()
                {
                    UserNotification = userNotification,
                    IsDeliverable = true,
                    AvailableToAdd = otherAddressAvailable,
                    UnDeliverableText = resources.GetResourceString("Kadena.Checkout.UndeliverableText"),
                    NewAddress = new NewAddressButton()
                    {
                        Label = resources.GetResourceString("Kadena.Checkout.NewAddress"),
                        Url = documents.GetDocumentUrl(resources.GetSettingsKey("KDA_SettingsPageUrl")) + "?tab=t4"
                    },
                    Title = resources.GetResourceString("Kadena.Checkout.DeliveryAddress.Title"),
                    Description = resources.GetResourceString("Kadena.Checkout.DeliveryDescription"),
                    EmptyMessage = resources.GetResourceString("Kadena.Checkout.NoAddressesMessage"),
                    items = addresses.ToList(),
                    DialogUI = GetOtherAddressDialog(),
                    Bounds = new DeliveryAddressesBounds
                    {
                        Limit = 3,
                        ShowLessText = resources.GetResourceString("Kadena.Checkout.DeliveryAddress.ShowLess"),
                        ShowMoreText = resources.GetResourceString("Kadena.Checkout.DeliveryAddress.ShowMore")
                    }
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
                    IsDisabled = false
                },
                ValidationMessage = resources.GetResourceString("Kadena.Checkout.ValidationError"),
                EmailConfirmation = new NotificationEmail
                {
                    Exists = bool.Parse(resources.GetSettingsKey("KDA_UseNotificationEmailsOnCheckout")),
                    MaxItems = int.Parse(resources.GetSettingsKey("KDA_MaximumNotificationEmailsOnCheckout")),
                    TooltipText = new NotificationEmailTooltip
                    {
                        Add = resources.GetResourceString("Kadena.Checkout.AddEmail"),
                        Remove = resources.GetResourceString("Kadena.Checkout.RemoveEmail")
                    },
                    Title = resources.GetResourceString("Kadena.Checkout.EmailTitle"),
                    Description = resources.GetResourceString("Kadena.Checkout.EmailDescription")
                }
            };

            CheckCurrentOrDefaultAddress(checkoutPage);
            checkoutPage.PaymentMethods.CheckDefault();
            checkoutPage.PaymentMethods.CheckPayability();
            checkoutPage.SetDisplayType();
            SetPricesVisibility(checkoutPage);
            return checkoutPage;
        }

        private Models.Checkout.AddressDialog GetOtherAddressDialog()
        {
            var countries = kenticoProvider.GetCountries();
            var states = kenticoProvider.GetStates();
            var defaultCountryId = int.Parse(resources.GetSettingsKey("KDA_AddressDefaultCountry"));
            return new Models.Checkout.AddressDialog
            {
                Title = resources.GetResourceString("Kadena.Checkout.NewAddress"),
                DiscardBtnLabel = resources.GetResourceString("Kadena.Settings.Addresses.DiscardChanges"),
                SubmitBtnLabel = resources.GetResourceString("Kadena.Settings.Addresses.SaveAddress"),
                RequiredErrorMessage = resources.GetResourceString("Kadena.Settings.RequiredField"),
                Fields = new[] {
                    new DialogField
                    {
                        Id = "customerName",
                        Label = resources.GetResourceString("Kadena.Settings.CustomerName"),
                        Type = "text"
                    },
                    new DialogField
                    {
                        Id = "address1",
                        Label = resources.GetResourceString("Kadena.Settings.Addresses.AddressLine1"),
                        Type = "text"
                    },
                    new DialogField
                    {
                        Id = "address2",
                        Label = resources.GetResourceString("Kadena.Settings.Addresses.AddressLine2"),
                        IsOptional = true,
                        Type = "text"
                    },
                    new DialogField
                    {
                        Id = "city",
                        Label = resources.GetResourceString("Kadena.Settings.Addresses.City"),
                        Type = "text"
                    },
                    new DialogField
                    {
                        Id = "state",
                        Label = resources.GetResourceString("Kadena.Settings.Addresses.State"),
                        IsOptional = true,
                        Type = "select",
                        Values = new List<object>()
                    },
                    new DialogField
                    {
                        Id = "zip",
                        Label = resources.GetResourceString("Kadena.Settings.Addresses.Zip"),
                        Type = "text"
                    },
                    new DialogField
                    {
                        Id = "country",
                        Label = resources.GetResourceString("Kadena.Settings.Addresses.Country"),
                        Type = "select",
                        Values = countries
                                .GroupJoin(states, c => c.Id, s => s.CountryId, (c, sts) => (object) new
                                {
                                    Id = c.Id.ToString(),
                                    Name = c.Name,
                                    IsDefault = (c.Id == defaultCountryId),
                                    Values = sts.Select(s => new
                                    {
                                        Id = s.Id.ToString(),
                                        Name = s.StateCode
                                    }).ToArray()
                                }).ToList()
                    },
                    new DialogField
                    {
                        Id = "phone",
                        Label = resources.GetResourceString("Kadena.ContactForm.Phone"),
                        IsOptional = true,
                        Type = "text"
                    },
                    new DialogField
                    {
                        Id = "email",
                        Label = resources.GetResourceString("Kadena.ContactForm.Email"),
                        Type = "text"
                    }
                }
            };
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
    }
}
