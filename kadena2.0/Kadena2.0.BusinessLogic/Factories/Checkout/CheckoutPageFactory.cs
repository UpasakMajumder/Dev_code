using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Settings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.BusinessLogic.Factories.Checkout
{
    public class CheckoutPageFactory : ICheckoutPageFactory
    {
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly IKenticoProviderService kenticoProvider;

        public CheckoutPageFactory(IKenticoResourceService resources, IKenticoDocumentProvider documents, IKenticoProviderService kenticoProvider)
        {
            if(resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }
            if (kenticoProvider == null)
            {
                throw new ArgumentNullException(nameof(kenticoProvider));
            }


            this.resources = resources;
            this.documents = documents;
            this.kenticoProvider = kenticoProvider;
        }

        public CartEmptyInfo CreateCartEmptyInfo(CartItem[] cartItems)
        {
            if (cartItems != null && cartItems.Length > 0)
                return null;

            return new CartEmptyInfo
            {
                Text = resources.GetResourceString("Kadena.Checkout.CartIsEmpty"),
                DashboardButtonText = resources.GetResourceString("Kadena.Checkout.ButtonDashboard"),
                DashboardButtonUrl = documents.GetDocumentUrl(resources.GetSettingsKey("KDA_EmptyCart_DashboardUrl")),
                ProductsButtonText = resources.GetResourceString("Kadena.Checkout.ButtonProducts"),
                ProductsButtonUrl = documents.GetDocumentUrl(resources.GetSettingsKey("KDA_EmptyCart_ProductsUrl"))
            };
        }

        public CartItems CreateProducts(CartItem[] cartItems, ShoppingCartTotals cartItemsTotals, string countOfItemsString)
        {
            var count = cartItems?.Length ?? 0;

            return new CartItems()
            {
                Number = string.Format(resources.GetResourceString("Kadena.Checkout.CountOfItems"), count, countOfItemsString),
                Items = cartItems.ToList(),
                ButtonLabels = new ButtonLabels
                {
                    Edit = resources.GetResourceString("Kadena.Checkout.EditButton"),
                    Remove = resources.GetResourceString("Kadena.Checkout.RemoveButton"),
                },
                SummaryPrice = CreateCartPrice(cartItemsTotals)
            };
        }

        public CartPrice CreateCartPrice(ShoppingCartTotals cartItemsTotals)
        {
            return new CartPrice
            {
                PricePrefix = resources.GetResourceString("Kadena.Checkout.ItemPricePrefix"),
                Price = string.Format("{0:#,0.00}", cartItemsTotals.TotalItemsPrice)
            };
        }

        public DeliveryAddresses CreateDeliveryAddresses(List<DeliveryAddress> addresses, string userNotificationString, bool otherAddressAvailable)
        {
            return new DeliveryAddresses()
            {
                UserNotification = userNotificationString,
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
            };
        }

        public Models.Checkout.AddressDialog GetOtherAddressDialog()
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

        public PaymentMethods CreatePaymentMethods(PaymentMethod[] paymentMethods)
        {
            return new PaymentMethods()
            {
                IsPayable = true,
                UnPayableText = resources.GetResourceString("Kadena.Checkout.UnpayableText"),
                Title = resources.GetResourceString("Kadena.Checkout.Payment.Title"),
                Items = ArrangePaymentMethods(paymentMethods)
            };
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

        public SubmitButton CreateSubmitButton()
        {
            return new SubmitButton()
            {
                BtnLabel = resources.GetResourceString("Kadena.Checkout.ButtonPlaceOrder"),
                DisabledText = resources.GetResourceString("Kadena.Checkout.ButtonWaitingForTemplateService"),
                IsDisabled = false
            };
        }
    }
}