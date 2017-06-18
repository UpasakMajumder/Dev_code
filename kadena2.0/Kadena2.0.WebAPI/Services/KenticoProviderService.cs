using Kadena.WebAPI.Contracts;
using CMS.Ecommerce;
using Kadena.WebAPI.Models;
using AutoMapper;
using System.Linq;
using CMS.SiteProvider;
using CMS.Helpers;
using System;
using CMS.DataEngine;
using CMS.Globalization;
using System.Collections.Generic;

namespace Kadena.WebAPI.Services
{
    public class KenticoProviderService : IKenticoProviderService
    {
        private readonly IMapper mapper;
        private readonly IKenticoResourceService resources;

        public KenticoProviderService(IMapper mapper, IKenticoResourceService resources)
        {
            this.mapper = mapper;
            this.resources = resources;
        }

        public DeliveryAddress[] GetCustomerAddresses(string addressType = null)
        {
            var customer = ECommerceContext.CurrentCustomer;
            var query = AddressInfoProvider.GetAddresses(customer.CustomerID);
            if (!string.IsNullOrWhiteSpace(addressType))
            {
                query = query.Where($"AddressType ='{addressType}'");
            }
            var addresses = query.ToArray();
            return mapper.Map<DeliveryAddress[]>(addresses);
        }

        public DeliveryAddress[] GetCustomerShippingAddresses(int customerId)
        {
            var addresses = AddressInfoProvider.GetAddresses(customerId)
                .Where(a => a.GetStringValue("AddressType", string.Empty) == "Shipping")
                .ToArray();
            return mapper.Map<DeliveryAddress[]>(addresses);
        }

        public DeliveryAddress GetCurrentCartShippingAddress()
        {
            var address = ECommerceContext.CurrentShoppingCart.ShoppingCartShippingAddress;

            if (address == null)
                return null;

            return mapper.Map<DeliveryAddress>(address);            
        }

        public BillingAddress GetDefaultBillingAddress()
        {
            var streets = new[]
            {
                SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderAddressLine1"),
                SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderAddressLine2")
            }.Where(i => !string.IsNullOrEmpty(i)).ToList();

            string countryName = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderCountry");
            string stateName = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderState");
            int countryId = CountryInfoProvider.GetCountryInfoByCode(countryName).CountryID;
            int stateId = StateInfoProvider.GetStateInfoByCode(stateName).StateID;

            return new BillingAddress()
            {
                Street = streets,
                City = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderCity"),
                Country = countryName,
                CountryId = countryId,
                Zip = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderPostal"),
                State = stateName,
                StateId = stateId
            };
        }

        public DeliveryCarrier[] GetShippingCarriers()
        {
            var shippingOptions = GetShippingOptions();
            var carriers = CarrierInfoProvider.GetCarriers(SiteContext.CurrentSiteID).ToArray();

            var deliveryMethods = mapper.Map<DeliveryCarrier[]>(carriers);

            foreach (DeliveryCarrier dm in deliveryMethods)
            {
                dm.SetShippingOptions(shippingOptions);
                SetShippingProviderIcon(dm);
            }

            return deliveryMethods;
        }

        /// <summary>
        /// Hardcoded until finding some convinient way to configure it in Kentico
        /// </summary>
        private void SetShippingProviderIcon(DeliveryCarrier dm)
        {
            if (dm.Title.ToLower().Contains("fedex"))
            {
                dm.Icon = "fedex-delivery";
            }
            else if (dm.Title.ToLower().Contains("usps"))
            {
                dm.Icon = "usps-delivery";
            }
            else if (dm.Title.ToLower().Contains("ups"))
            {
                dm.Icon = "ups-delivery";
            }
        }

        public DeliveryOption[] GetShippingOptions()
        {
            var services = ShippingOptionInfoProvider.GetShippingOptions(SiteContext.CurrentSiteID).Where(s => s.ShippingOptionEnabled).ToArray();
            var result = mapper.Map<DeliveryOption[]>(services);
            GetShippingPrice(result);
            return result;
        }

        public DeliveryOption GetShippingOption(int id)
        {
            var service = ShippingOptionInfoProvider.GetShippingOptionInfo(id);
            var result = mapper.Map<DeliveryOption>(service);
            var carrier = CarrierInfoProvider.GetCarrierInfo(service.ShippingOptionCarrierID);
            result.CarrierCode = carrier.CarrierName;
            return result;
        }

        private void GetShippingPrice(DeliveryOption[] services)
        {
            // this method's approach comes from origial kentico webpart (ShippingSeletion)
            int originalCartShippingId = ECommerceContext.CurrentShoppingCart.ShoppingCartShippingOptionID;

            foreach (var s in services)
            {
                var cart = ECommerceContext.CurrentShoppingCart;
                cart.ShoppingCartShippingOptionID = s.Id;
                s.PriceAmount = cart.TotalShipping;
                s.Price = String.Format("$ {0:#,0.00}", ECommerceContext.CurrentShoppingCart.TotalShipping);

                if (cart.TotalShipping == 0.0d && !s.IsCustomerPrice)
                {
                    s.Disabled = true;
                }
            }

            ECommerceContext.CurrentShoppingCart.ShoppingCartShippingOptionID = originalCartShippingId;
        }


        public PaymentMethod[] GetPaymentMethods()
        {
            var methods = PaymentOptionInfoProvider.GetPaymentOptions(SiteContext.CurrentSiteID).Where(p => p.PaymentOptionEnabled).ToArray();            
            return mapper.Map<PaymentMethod[]>(methods);
        }

        public PaymentMethod GetPaymentMethod(int id)
        {
            var method = PaymentOptionInfoProvider.GetPaymentOptionInfo(id);
            return mapper.Map<PaymentMethod>(method);
        }

        public ShoppingCartTotals GetShoppingCartTotals()
        {
            return new ShoppingCartTotals()
            {
                TotalItemsPrice = ECommerceContext.CurrentShoppingCart.TotalItemsPrice,
                TotalShipping = ECommerceContext.CurrentShoppingCart.TotalShipping,
                TotalTax = ECommerceContext.CurrentShoppingCart.TotalTax
            };
        }

        public void SetShoppingCartAddres(int addressId)
        {
            var cart = ECommerceContext.CurrentShoppingCart;

            if (cart.ShoppingCartShippingAddress == null || cart.ShoppingCartShippingAddress.AddressID != addressId)
            {
                var address = AddressInfoProvider.GetAddressInfo(addressId);
                cart.ShoppingCartShippingAddress = address;
                cart.SubmitChanges(true);
            }
        }

        public void SelectShipping(int shippingOptionId)
        {
            var cart = ECommerceContext.CurrentShoppingCart;

            if (cart.ShoppingCartShippingOptionID != shippingOptionId)
            {
                cart.ShoppingCartShippingOptionID = shippingOptionId;
                cart.SubmitChanges(true);
            }
        }

        public int GetCurrentCartAddresId()
        {
            var address = ECommerceContext.CurrentShoppingCart.ShoppingCartShippingAddress;
            return address?.AddressID ?? 0;
        }

        public int GetCurrentCartShippingOptionId()
        {
            return ECommerceContext.CurrentShoppingCart.ShoppingCartShippingOptionID;
        }

        public Customer GetCurrentCustomer()
        {
            var customer = ECommerceContext.CurrentCustomer;

            if (customer == null)
                return null;

            return new Customer()
            {
                Id = customer.CustomerID,
                FirstName = customer.CustomerFirstName,
                LastName = customer.CustomerLastName,
                Email = customer.CustomerEmail,
                CustomerNumber = customer.CustomerGUID.ToString(),
                Phone = customer.CustomerPhone,
                UserID = customer.CustomerUserID,
                Company = customer.CustomerCompany
            };
        }

        public Customer GetCustomer(int customerId)
        {
            var customer = CustomerInfoProvider.GetCustomerInfo(customerId);

            if (customer == null)
                return null;

            return new Customer()
            {
                Id = customer.CustomerID,
                FirstName = customer.CustomerFirstName,
                LastName = customer.CustomerLastName,
                Email = customer.CustomerEmail,
                CustomerNumber = customer.CustomerGUID.ToString(),
                Phone = customer.CustomerPhone,
                UserID = customer.CustomerUserID,
                Company = customer.CustomerCompany
            };
        }

        public CartItem[] GetShoppingCartOrderItems()
        {
            var items = ECommerceContext.CurrentShoppingCart.CartItems;
            var result = items.Select(i => new CartItem()
            {
                Id = i.CartItemID,
                DesignFilePath = i.GetValue("DesignFilePath", string.Empty),
                MailingListGuid = i.GetValue("MailingListGuid", Guid.Empty), // seem to be redundant parameter, microservice doesn't use it
                ChilliEditorTemplateId = i.GetValue("ChilliEditorTemplateID", Guid.Empty),
                ChilliTemplateId = i.GetValue("ChiliTemplateID", Guid.Empty),
                ProductType = i.GetValue("ProductType", string.Empty),
                SKUName = i.SKU?.SKUName,
                SKUNumber = i.SKU?.SKUNumber,
                SKUID = i.SKUID,
                TotalPrice = i.TotalPrice,
                TotalTax = 0.0d,
                UnitPrice = i.UnitPrice,
                Quantity = i.CartItemUnits,
                UnitOfMeasure = "EA",
                DesignFilePathObtained = i.GetValue("DesignFilePathObtained", false),
                DesignFilePathTaskId = i.GetStringValue("DesignFilePathTaskId", string.Empty)
            }
            ).ToArray();

            return result;
        }

        public CartItem[] GetShoppingCartItems()
        {
            var items = ECommerceContext.CurrentShoppingCart.CartItems;
            var result = items.Select(i => new CartItem()
            {
                Id = i.CartItemID,
                Image = URLHelper.GetAbsoluteUrl(i.SKU.SKUImagePath),
                ProductType = i.GetValue("ProductType", string.Empty),
                Quantity = i.CartItemUnits,
                TotalPrice = i.UnitPrice * i.CartItemUnits,
                PricePrefix = resources.GetResourceString("Kadena.Checkout.ItemPricePrefix"),
                QuantityPrefix = resources.GetResourceString("Kadena.Checkout.QuantityPrefix"),
                Delivery = "", //TODO not known yet
                MailingListName = i.GetValue("MailingListName", string.Empty),
                MailingListGuid = i.GetValue("MailingListGuid", Guid.Empty),
                Template = i.SKU.SKUName,
                EditorTemplateId = i.GetValue("ChilliEditorTemplateID", string.Empty),
                ProductPageId = i.GetIntegerValue("ProductPageID", 0),
                SKUID = i.SKUID,
                StockQuantity = i.SKU.SKUAvailableItems
            }
            ).ToArray();

            return result;
        }

        public void RemoveCartItem(int id)
        {
            // Method approach inspired by \CMS\CMSModules\Ecommerce\Controls\Checkout\CartItemRemove.ascx.cs

            var cart = ECommerceContext.CurrentShoppingCart;
            var item = cart.CartItems.FirstOrDefault(i => i.CartItemID == id);

            if (item == null)
                return;

            // Delete all the children from the database if available        
            foreach (ShoppingCartItemInfo scii in cart.CartItems)
            {
                if ((scii.CartItemBundleGUID == item.CartItemGUID) || (scii.CartItemParentGUID == item.CartItemGUID))
                {
                    ShoppingCartItemInfoProvider.DeleteShoppingCartItemInfo(scii);
                }
            }

            // Deletes the CartItem from the database
            ShoppingCartItemInfoProvider.DeleteShoppingCartItemInfo(item.CartItemGUID);
            // Delete the CartItem form the shopping cart object (session)
            ShoppingCartInfoProvider.RemoveShoppingCartItem(cart, item.CartItemGUID);

            // Recalculate shopping cart
            ShoppingCartInfoProvider.EvaluateShoppingCart(cart);
        }

        public void SetCartItemDesignFilePath(int id, string path)
        {
            var cart = ECommerceContext.CurrentShoppingCart;
            var item = ECommerceContext.CurrentShoppingCart.CartItems.Where(i => i.CartItemID == id).FirstOrDefault();

            if (item != null)
            {
                item.SetValue("DesignFilePathObtained", true);
                item.SetValue("DesignFilePath", path);
                item.SubmitChanges(false);
            }
        }

        public void SetCartItemQuantity(int id, int quantity)
        {
            var cart = ECommerceContext.CurrentShoppingCart;
            var item = ECommerceContext.CurrentShoppingCart.CartItems.Where(i => i.CartItemID == id).FirstOrDefault();

            if (item == null)
            {
                throw new ArgumentOutOfRangeException($"item: {id}", "Item not found");
            }

            if (quantity < 1)
            {
                throw new ArgumentOutOfRangeException($"quantity: {quantity}", "Failed to set negative quantity");
            }

            var productType = item.GetStringValue("ProductType", string.Empty);

            if (!productType.Contains("KDA.InventoryProduct") && !productType.Contains("KDA.POD") && !productType.Contains("KDA.StaticProduct"))
            {
                throw new Exception($"Unable to set quantity for this product type");
            }

            if (productType.Contains("KDA.InventoryProduct") && quantity > item.SKU.SKUAvailableItems)
            {
                throw new ArgumentOutOfRangeException($"quantity: {quantity}, item: {id}", "Failed to set cart item quantity for InventoryProduct");
            }

            ShoppingCartItemInfoProvider.UpdateShoppingCartItemUnits(item, quantity);
            cart.InvalidateCalculations();
            ShoppingCartInfoProvider.EvaluateShoppingCart(cart);
        }

        public void RemoveCurrentItemsFromStock()
        {
            var items = ECommerceContext.CurrentShoppingCart.CartItems;

            foreach (var i in items)
            {
                if (i.GetValue("ProductType", string.Empty).Contains("KDA.InventoryProduct"))
                {
                    int toRemove = i.CartItemUnits <= i.SKU.SKUAvailableItems ? i.CartItemUnits : i.SKU.SKUAvailableItems;
                    i.SKU.SKUAvailableItems -= toRemove;
                    i.SKU.SubmitChanges(false);
                }
            }
        }

        public void RemoveCurrentItemsFromCart()
        {
            ShoppingCartInfoProvider.EmptyShoppingCart(ECommerceContext.CurrentShoppingCart);
        }

        public IEnumerable<State> GetStates()
        {
            return StateInfoProvider
                .GetStates()
                .Column("StateCode")
                .Select(s => new State
                {
                    StateCode = s["StateCode"].ToString()
                });
        }

        public void SaveShippingAddress(DeliveryAddress address)
        {
            var customer = ECommerceContext.CurrentCustomer;
            var state = StateInfoProvider.GetStateInfoByCode(address.State);
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state), "Incorrect state was selected.");
            }
            var info = new AddressInfo
            {
                AddressID = address.Id,
                AddressLine1 = address.Street.Count > 0 ? address.Street[0] : null,
                AddressLine2 = address.Street.Count > 1 ? address.Street[1] : null,
                AddressCity = address.City,
                AddressStateID = state.StateID,
                AddressCountryID = state.CountryID,
                AddressZip = address.Zip,
                AddressCustomerID = customer.CustomerID,
                AddressPersonalName = $"{customer.CustomerFirstName} {customer.CustomerLastName}"
            };
            info.AddressName = $"{info.AddressPersonalName}, {info.AddressLine1}, {info.AddressCity}";
            info.SetValue("AddressType", "Shipping");

            AddressInfoProvider.SetAddressInfo(info);
            address.Id = info.AddressID;
        }

        public double GetCurrentCartTotalItemsPrice()
        {
            return ECommerceContext.CurrentShoppingCart.TotalItemsPrice;
        }

        public double GetCurrentCartShippingCost()
        {
            return ECommerceContext.CurrentShoppingCart.Shipping;
        }
    }
}