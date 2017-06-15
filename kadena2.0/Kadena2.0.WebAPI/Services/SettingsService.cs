using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.Models;
using Kadena.WebAPI.Models.Settings;
using CMS.Helpers;
using System.Linq;
using System.Collections.Generic;

namespace Kadena.WebAPI.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IKenticoProviderService _kentico;

        public SettingsService(IKenticoProviderService kentico)
        {
            _kentico = kentico;
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
                ////    Title = ResHelper.GetString("Kadena.Settings.Addresses.BillingAddress"),
                ////    AddButton = new 
                ////    {
                ////        Exists = false,
                ////        Tooltip = ResHelper.GetString("Kadena.Settings.Addresses.AddBilling")
                ////    },
                ////    Addresses = billingAddresses.ToList()
                ////},
                Shipping = new AddressList
                {
                    Title = ResHelper.GetString("Kadena.Settings.Addresses.ShippingAddresses"),
                    AddButton = new PageButton
                    {
                        Exists = false,
                        Tooltip = ResHelper.GetString("Kadena.Settings.Addresses.AddShipping")
                    },
                    EditButtonText = ResHelper.GetString("Kadena.Settings.Addresses.Edit"),
                    RemoveButtonText = ResHelper.GetString("Kadena.Settings.Addresses.Remove"),
                    Addresses = shippingAddresses.ToList()
                },
                Dialog = new AddressDialog
                {
                    Types = new DialogType
                    {
                        Add = ResHelper.GetString("Kadena.Settings.Addresses.AddAddress"),
                        Edit = ResHelper.GetString("Kadena.Settings.Addresses.EditAddress")
                    },
                    Buttons = new DialogButton
                    {
                        Discard = ResHelper.GetString("Kadena.Settings.Addresses.DiscardChanges"),
                        Save = ResHelper.GetString("Kadena.Settings.Addresses.SaveAddress")
                    },
                    Fields = new List<DialogField> {
                        new DialogField { Id="street1",
                            Label = ResHelper.GetString("Kadena.Settings.Addresses.AddressLine1"),
                            Type = "text"},
                        new DialogField { Id="street2",
                            Label = ResHelper.GetString("Kadena.Settings.Addresses.AddressLine2"),
                            Type = "text"},
                        new DialogField { Id="city",
                            Label = ResHelper.GetString("Kadena.Settings.Addresses.City"),
                            Type = "text"},
                        new DialogField { Id="state",
                            Label = ResHelper.GetString("Kadena.Settings.Addresses.State"),
                            Type = "select",
                            Values = states.Select(s=>(object)s.StateCode).ToList() },
                        new DialogField { Id="zip",
                            Label = ResHelper.GetString("Kadena.Settings.Addresses.Zip"),
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