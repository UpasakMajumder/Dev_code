using Kadena.Models;
using Kadena.Models.Settings;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using System.Linq;
using System;
using Kadena2.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IKenticoPermissionsProvider _permissions;
        private readonly IKenticoLocalizationProvider _localization;
        private readonly IKenticoSiteProvider _site;
        private readonly IKenticoUserProvider _kenticoUsers;
        private readonly IKenticoResourceService _resources;
        private readonly IKenticoAddressBookProvider  _addresses;

        public SettingsService(IKenticoPermissionsProvider permissions,
                               IKenticoLocalizationProvider localization,
                               IKenticoSiteProvider site,
                               IKenticoUserProvider kenticoUsers, 
                               IKenticoResourceService resources,
                               IKenticoAddressBookProvider addresses)
        {
            if (permissions == null)
            {
                throw new ArgumentNullException(nameof(permissions));
            }
            if (localization == null)
            {
                throw new ArgumentNullException(nameof(localization));
            }
            if (site == null)
            {
                throw new ArgumentNullException(nameof(site));
            }
            if (kenticoUsers == null)
            {
                throw new ArgumentNullException(nameof(kenticoUsers));
            }
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (addresses == null)
            {
                throw new ArgumentNullException(nameof(addresses));
            }

            _permissions = permissions;
            _localization = localization;
            _site = site;
            _kenticoUsers = kenticoUsers;
            _resources = resources;
            _addresses = addresses;
        }

        public SettingsAddresses GetAddresses()
        {
            var customer = _kenticoUsers.GetCurrentCustomer();
            var billingAddresses = _addresses.GetCustomerAddresses(AddressType.Billing);
            var shippingAddresses = _addresses.GetCustomerAddresses(AddressType.Shipping);
            var shippingAddressesSorted = shippingAddresses
                .Where(sa => sa.Id == customer.DefaultShippingAddressId)
                .Concat(shippingAddresses.Where(sa => sa.Id != customer.DefaultShippingAddressId))
                .ToList();
            var states = _localization.GetStates();
            var countries = _localization.GetCountries();
            var canEdit = _permissions.UserCanModifyShippingAddress();
            var maxShippingAddressesSetting = _resources.GetSettingsKey("KDA_ShippingAddressMaxLimit");

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
                    Fields = new List<DialogField> {
                        new DialogField {
                            Id = "address1",
                            Label = _resources.GetResourceString("Kadena.Settings.Addresses.AddressLine1"),
                            Type = "text"},
                        new DialogField {
                            Id = "address2",
                            Label = _resources.GetResourceString("Kadena.Settings.Addresses.AddressLine2"),
                            Type = "text",
                            IsOptional = true
                        },
                        new DialogField {
                            Id = "city",
                            Label = _resources.GetResourceString("Kadena.Settings.Addresses.City"),
                            Type = "text"
                        },
                        new DialogField {
                            Id = "state",
                            Label = _resources.GetResourceString("Kadena.Settings.Addresses.State"),
                            Type = "select",
                            Values = new List<object>()
                        },
                        new DialogField {
                            Id = "zip",
                            Label = _resources.GetResourceString("Kadena.Settings.Addresses.Zip"),
                            Type = "text"
                        } ,
                        new DialogField {
                            Id = "country",
                            Label = "Country",
                            Values = countries
                                .GroupJoin(states, c => c.Id, s => s.CountryId, (c, sts) => (object) new
                                {
                                    Id = c.Id.ToString(),
                                    Name = c.Name,
                                    Values = sts.Select(s => new
                                    {
                                        Id = s.Id.ToString(),
                                        Name = s.StateDisplayName
                                    }).ToArray()
                                }).ToList()
                        }
                    }
                }
            };
        }

        public bool SaveLocalization(string code)
        {
            return _kenticoUsers.SaveLocalization(code);
        }

        public void SaveShippingAddress(DeliveryAddress address)
        {
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