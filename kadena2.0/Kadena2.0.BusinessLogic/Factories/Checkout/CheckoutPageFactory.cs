using Kadena.BusinessLogic.Contracts;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Settings;
using Kadena.Models.SiteSettings;
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
        private readonly IKenticoLocalizationProvider kenticoLocalization;
        private readonly IDialogService dialogService;

        public CheckoutPageFactory(IKenticoResourceService resources, IKenticoDocumentProvider documents, 
            IKenticoLocalizationProvider kenticoLocalization, IDialogService dialogService)
        {
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.documents = documents ?? throw new ArgumentNullException(nameof(documents));
            this.kenticoLocalization = kenticoLocalization ?? throw new ArgumentNullException(nameof(kenticoLocalization));
            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        public CartEmptyInfo CreateCartEmptyInfo()
        {
            return new CartEmptyInfo
            {
                Text = resources.GetResourceString("Kadena.Checkout.CartIsEmpty"),
                DashboardButtonText = resources.GetResourceString("Kadena.Checkout.ButtonDashboard"),
                DashboardButtonUrl = documents.GetDocumentUrl(resources.GetSiteSettingsKey(Settings.KDA_EmptyCart_DashboardUrl)),
                ProductsButtonText = resources.GetResourceString("Kadena.Checkout.ButtonProducts"),
                ProductsButtonUrl = documents.GetDocumentUrl(resources.GetSiteSettingsKey(Settings.KDA_EmptyCart_ProductsUrl))
            };
        }

        public CartItems CreateProducts(List<CheckoutCartItem> cartItems, ShoppingCartTotals cartItemsTotals, string countOfItemsString)
        {
            var count = cartItems?.Count ?? 0;

            return new CartItems()
            {
                Number = string.Format(resources.GetResourceString("Kadena.Checkout.CountOfItems"), count, countOfItemsString),
                Items = cartItems.ToList(),
                ButtonLabels = new ButtonLabels
                {
                    Edit = resources.GetResourceString("Kadena.Checkout.EditButton"),
                    Remove = resources.GetResourceString("Kadena.Checkout.RemoveButton")
                },
                SummaryPrice = CreateCartPrice(cartItemsTotals),
                ProductionTimeLabel = resources.GetResourceString("Kadena.Checkout.ProductionTimeLabel"),
                ShipTimeLabel = resources.GetResourceString("Kadena.Checkout.ShipTimeLabel")
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
                    Url = documents.GetDocumentUrl(resources.GetSiteSettingsKey(Settings.KDA_SettingsPageUrl)) + "?tab=t4"
                },
                Title = resources.GetResourceString("Kadena.Checkout.DeliveryAddress.Title"),
                Description = resources.GetResourceString("Kadena.Checkout.DeliveryDescription"),
                EmptyMessage = resources.GetResourceString("Kadena.Checkout.NoAddressesMessage"),
                items = addresses,
                DialogUI = GetOtherAddressDialog(),
                Bounds = new DeliveryAddressesBounds
                {
                    Limit = 3,
                    ShowLessText = resources.GetResourceString("Kadena.Checkout.DeliveryAddress.ShowLess"),
                    ShowMoreText = resources.GetResourceString("Kadena.Checkout.DeliveryAddress.ShowMore")
                }
            };
        }

        private Models.Checkout.AddressDialog GetOtherAddressDialog()
        {
            return new Models.Checkout.AddressDialog
            {
                Title = resources.GetResourceString("Kadena.Checkout.NewAddress"),
                DiscardBtnLabel = resources.GetResourceString("Kadena.Settings.Addresses.DiscardChanges"),
                SubmitBtnLabel = resources.GetResourceString("Kadena.Settings.Addresses.SaveAddress"),
                RequiredErrorMessage = resources.GetResourceString("Kadena.Settings.RequiredField"),
                SaveAddressCheckbox = resources.GetResourceString("Kadena.Checkout.PersistAddressCheckbox"),
                Fields = dialogService.GetAddressFields()
            };
        }

        public PaymentMethods CreatePaymentMethods(PaymentMethod[] paymentMethods)
        {
            return new PaymentMethods()
            {
                IsPayable = true,
                UnPayableText = resources.GetResourceString("Kadena.Checkout.UnpayableText"),
                Title = resources.GetResourceString("Kadena.Checkout.Payment.Title"),
                ApprovalRequiredText = resources.GetResourceString("KDA.PaymentMethods.RequiresApprovalTitle"),
                ApprovalRequiredDesc = resources.GetResourceString("KDA.PaymentMethods.RequiresApprovalPopUpText"),
                ApprovalRequiredButton = resources.GetResourceString("KDA.PaymentMethods.RequiresApprovalPopUpConfirmButtonText"),
                Items = ArrangePaymentMethods(paymentMethods)
            };
        }

        private List<PaymentMethod> ArrangePaymentMethods(PaymentMethod[] allMethods)
        {
            var purchaseOrderMethod = allMethods.Where(m => m.ClassName.Contains("PurchaseOrder")).FirstOrDefault();
            if (purchaseOrderMethod != null)
            {
                purchaseOrderMethod.HasInput = true;
                purchaseOrderMethod.InputPlaceholder = resources.GetPerSiteResourceString("Kadena.Checkout.InsertPONumber");
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

        public DeliveryDate CreateDeliveryDateInput()
        {
            return resources.GetSiteSettingsKey<bool>(Settings.KDA_CartRequestDateEnabled) ?
                new DeliveryDate
                {
                    Title = resources.GetResourceString("Kadena.Checkout.DeliveryDate.Title"),
                    Messages = new DeliveryDateMessages
                    {
                        Invalid = resources.GetResourceString("Kadena.Checkout.DeliveryDate.Messages.Invalid"),
                        Upcoming = resources.GetResourceString("Kadena.Checkout.DeliveryDate.Upcoming")
                    }
                } : null;
        }

        public NotificationEmail CreateNotificationEmail(bool emailConfirmationEnabled)
        {
            int maxitems = 0;
            int.TryParse(resources.GetSiteSettingsKey(Settings.KDA_MaximumNotificationEmailsOnCheckout), out maxitems);

            return new NotificationEmail
            {
                Exists = emailConfirmationEnabled,
                MaxItems = maxitems,
                TooltipText = new NotificationEmailTooltip
                {
                    Add = resources.GetResourceString("Kadena.Checkout.AddEmail"),
                    Remove = resources.GetResourceString("Kadena.Checkout.RemoveEmail")
                },
                Title = resources.GetResourceString("Kadena.Checkout.EmailTitle"),
                Description = resources.GetResourceString("Kadena.Checkout.EmailDescription")
            };
        }
    }
}