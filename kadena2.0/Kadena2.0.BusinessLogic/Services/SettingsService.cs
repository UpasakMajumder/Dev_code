using Kadena.Models;
using Kadena.Models.Settings;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Linq;
using System;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.SiteSettings;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;

namespace Kadena.BusinessLogic.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IKenticoPermissionsProvider _permissions;
        private readonly IKenticoLocalizationProvider _localization;
        private readonly IKenticoSiteProvider _site;
        private readonly IKenticoUserProvider _kenticoUsers;
        private readonly IKenticoCustomerProvider _kenticoCustomers;
        private readonly IKenticoResourceService _resources;
        private readonly IKenticoAddressBookProvider _addresses;
        private readonly IDialogService _dialogService;
        private readonly IKadenaSettings _kadenaSettings;

        public SettingsService(IKenticoPermissionsProvider permissions,
                               IKenticoLocalizationProvider localization,
                               IKenticoSiteProvider site,
                               IKenticoUserProvider kenticoUsers,
                               IKenticoCustomerProvider kenticoCustomers,
                               IKenticoResourceService resources,
                               IKenticoAddressBookProvider addresses,
                               IDialogService dialogService,
                               IKadenaSettings kadenaSettings)
        {
            _permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            _localization = localization ?? throw new ArgumentNullException(nameof(localization));
            _site = site ?? throw new ArgumentNullException(nameof(site));
            _kenticoUsers = kenticoUsers ?? throw new ArgumentNullException(nameof(kenticoUsers));
            _kenticoCustomers = kenticoCustomers ?? throw new ArgumentNullException(nameof(kenticoCustomers));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _addresses = addresses ?? throw new ArgumentNullException(nameof(addresses));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _kadenaSettings = kadenaSettings ?? throw new ArgumentNullException(nameof(kadenaSettings));
        }

        public SettingsAddresses GetAddresses()
        {
            var customer = _kenticoCustomers.GetCurrentCustomer();
            var billingAddresses = _addresses.GetCustomerAddresses(AddressType.Billing);
            var shippingAddresses = _addresses.GetCustomerAddresses(AddressType.Shipping);
            var shippingAddressesSorted = shippingAddresses
                .Where(sa => sa.Id == customer.DefaultShippingAddressId)
                .Concat(shippingAddresses.Where(sa => sa.Id != customer.DefaultShippingAddressId))
                .ToList();
            var states = _localization.GetStates();
            var countries = _localization.GetCountries();
            var canEdit = _permissions.UserCanModifyShippingAddress();
            var maxShippingAddressesSetting = _resources.GetSiteSettingsKey(Settings.KDA_ShippingAddressMaxLimit);

            var userNotification = string.Empty;
            var userNotificationLocalizationKey = _site.GetCurrentSiteCodeName() + ".Kadena.Settings.Address.NotificationMessage";
            if (!_localization.IsCurrentCultureDefault())
            {
                userNotification = _resources.GetResourceString(userNotificationLocalizationKey) == userNotificationLocalizationKey ? string.Empty : _resources.GetResourceString(userNotificationLocalizationKey);
            }

            var maxShippingAddresses = 3;
            if (!string.IsNullOrWhiteSpace(maxShippingAddressesSetting))
            {
                maxShippingAddresses = int.Parse(maxShippingAddressesSetting);
            }

            return new SettingsAddresses
            {
                Billing = new object(),
                ////// TODO Uncomment when billing addresses will be developed
                ////new Addresses
                ////{
                ////    Title = _resources.GetResourceString("Kadena.Settings.Addresses.BillingAddress"),
                ////    AddButton = new 
                ////    {
                ////        Exists = false,
                ////        Tooltip = _resources.GetResourceString("Kadena.Settings.Addresses.AddBilling")
                ////    },
                ////    Addresses = billingAddresses.ToList()
                ////},
                Shipping = new AddressList
                {
                    Title = _resources.GetResourceString("Kadena.Settings.Addresses.ShippingAddresses"),
                    AllowAddresses = maxShippingAddresses,
                    AddButton = new PageButton
                    {
                        Exists = true,
                        Text = _resources.GetResourceString("Kadena.Settings.Addresses.AddShipping")
                    },
                    EditButton = new PageButton
                    {
                        Exists = canEdit,
                        Text = _resources.GetResourceString("Kadena.Settings.Addresses.Edit")
                    },
                    RemoveButton = new PageButton
                    {
                        Exists = false,
                        Text = _resources.GetResourceString("Kadena.Settings.Addresses.Remove")
                    },
                    DefaultAddress = new DefaultAddress
                    {
                        Id = customer.DefaultShippingAddressId,
                        LabelDefault = _resources.GetResourceString("Kadena.Settings.Addresses.Primary"),
                        LabelNonDefault = _resources.GetResourceString("Kadena.Settings.Addresses.NotPrimary"),
                        Tooltip = _resources.GetResourceString("Kadena.Settings.Addresses.SetUnset"),
                        SetUrl = "api/usersettings/setdefaultshippingaddress",
                        UnsetUrl = "api/usersettings/unsetdefaultshippingaddress"
                    },
                    Addresses = shippingAddressesSorted
                },
                Dialog = new AddressDialog
                {
                    UserNotification = userNotification,
                    Types = new DialogType
                    {
                        Add = _resources.GetResourceString("Kadena.Settings.Addresses.AddAddress"),
                        Edit = _resources.GetResourceString("Kadena.Settings.Addresses.EditAddress")
                    },
                    Buttons = new DialogButton
                    {
                        Discard = _resources.GetResourceString("Kadena.Settings.Addresses.DiscardChanges"),
                        Save = _resources.GetResourceString("Kadena.Settings.Addresses.SaveAddress")
                    },
                    Fields = _dialogService.GetAddressFields().ToList()
                }
            };
        }

        public bool SaveLocalization(string code)
        {
            return _kenticoUsers.SaveLocalization(code);
        }

        public void SaveShippingAddress(DeliveryAddress address)
        {
            if (address.Country != null && address.Country.Id == 0)
            {
                address.Country = _localization.GetCountries()?.FirstOrDefault(c => c.Code == address.Country.Code);
            }

            if (address.State != null && address.State.Id == 0)
            {
                address.State = _localization.GetStates()?.FirstOrDefault(c => c.StateCode == address.State.StateCode);
            }

            var customer = address.CustomerId > 0
                ? _kenticoCustomers.GetCustomer(address.CustomerId)
                : _kenticoCustomers.GetCurrentCustomer();

            if (string.IsNullOrWhiteSpace(address.AddressPersonalName))
            {
                address.AddressPersonalName = customer?.FullName;
            }

            if (string.IsNullOrWhiteSpace(address.CompanyName))
            {
                if (!string.IsNullOrWhiteSpace(customer?.Company))
                {
                    address.CompanyName = customer.Company;
                }
                else
                {
                    address.CompanyName = _kadenaSettings.DefaultCustomerCompanyName;
                }
            }

            if (string.IsNullOrWhiteSpace(address.AddressName))
            {
                address.AddressName = $"{address.AddressPersonalName}, {address.Address1}, {address.City}";
            }

            address.CustomerId = customer?.Id ?? 0;
            _addresses.SaveShippingAddress(address);
        }

        public void SetDefaultShippingAddress(int addressId)
        {
            _addresses.SetDefaultShippingAddress(addressId);
        }

        public void UnsetDefaultShippingAddress()
        {
            _addresses.UnsetDefaultShippingAddress();
        }
    }
}