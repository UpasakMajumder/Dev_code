using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Models;
using Kadena.WebAPI.Models.Settings;
using System.Linq;
using System.Collections.Generic;

namespace Kadena.WebAPI.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IKenticoProviderService _kentico;
        private readonly IKenticoResourceService _resources;

        public SettingsService(IKenticoProviderService kentico, IKenticoResourceService resources)
        {
            _kentico = kentico;
            _resources = resources;
        }

        public SettingsAddresses GetAddresses()
        {
            var billingAddresses = _kentico.GetCustomerAddresses("Billing");
            var shippingAddresses = _kentico.GetCustomerAddresses("Shipping");
            var states = _kentico.GetStates();

            return new SettingsAddresses
            {
                Billing = new object(),
                //////Uncomment when billing addresses will be developed
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
                    AddButton = new PageButton
                    {
                        Exists = false,
                        Tooltip = _resources.GetResourceString("Kadena.Settings.Addresses.AddShipping")
                    },
                    EditButtonText = _resources.GetResourceString("Kadena.Settings.Addresses.Edit"),
                    RemoveButtonText = _resources.GetResourceString("Kadena.Settings.Addresses.Remove"),
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

        public void SaveShippingAddress(DeliveryAddress address)
        {
            _kentico.SaveShippingAddress(address);
        }
    }
}