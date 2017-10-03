using Kadena.WebAPI.Contracts;
using Kadena.Models;
using Kadena.Models.Settings;
using System.Linq;
using System.Collections.Generic;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.WebAPI.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IKenticoProviderService _kentico;
        private readonly IKenticoUserProvider _kenticoUsers;
        private readonly IKenticoResourceService _resources;

        public SettingsService(IKenticoProviderService kentico, IKenticoUserProvider kenticoUsers, IKenticoResourceService resources)
        {
            _kentico = kentico;
            _kenticoUsers = kenticoUsers;
            _resources = resources;
        }

        public SettingsAddresses GetAddresses()
        {
            var billingAddresses = _kenticoUsers.GetCustomerAddresses(AddressType.Billing);
            var shippingAddresses = _kenticoUsers.GetCustomerAddresses(AddressType.Shipping);
            var states = _kentico.GetStates();
            var canEdit = _kenticoUsers.UserCanModifyShippingAddress();
            var maxShippingAddressesSetting = _resources.GetSettingsKey("KDA_ShippingAddressMaxLimit");

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
                    Addresses = shippingAddresses.ToList()
                },
                Dialog = new AddressDialog
                {
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
                        new DialogField { Id="street1",
                            Label = _resources.GetResourceString("Kadena.Settings.Addresses.AddressLine1"),
                            Type = "text"},
                        new DialogField { Id="street2",
                            Label = _resources.GetResourceString("Kadena.Settings.Addresses.AddressLine2"),
                            Type = "text",
                            IsOptional = true},
                        new DialogField { Id="city",
                            Label = _resources.GetResourceString("Kadena.Settings.Addresses.City"),
                            Type = "text"},
                        new DialogField { Id="state",
                            Label = _resources.GetResourceString("Kadena.Settings.Addresses.State"),
                            Type = "select",
                            Values = states.Select(s=>(object)s.StateCode).ToList() },
                        new DialogField { Id="zip",
                            Label = _resources.GetResourceString("Kadena.Settings.Addresses.Zip"),
                            Type = "text"}
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
            _kentico.SaveShippingAddress(address);
        }

        public void SetDefaultShippingAddress(int addressId)
        {
            _kenticoUsers.SetDefaultShippingAddress(addressId);
        }
    }
}