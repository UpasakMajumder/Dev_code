﻿using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models;
using Kadena.Models.SubmitOrder;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using System;

namespace Kadena.BusinessLogic.Factories.Checkout
{
    public class OrderDataFactory : IOrderDataFactory
    {
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly IKenticoLocalizationProvider kenticoLocalization;
        private readonly IKadenaSettings settings;

        public OrderDataFactory(IKenticoResourceService resources, IKenticoDocumentProvider documents, IKenticoLocalizationProvider kenticoLocalization, IKadenaSettings settings)
        {
            if(resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }
            if (kenticoLocalization == null)
            {
                throw new ArgumentNullException(nameof(kenticoLocalization));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }


            this.resources = resources;
            this.documents = documents;
            this.kenticoLocalization = kenticoLocalization;
            this.settings = settings;
        }


        public AddressDTO CreateBillingAddress(BillingAddress billingAddress, string billingStateDisplayName)
        {
            return new AddressDTO()
            {
                AddressLine1 = billingAddress.Street.Count > 0 ? billingAddress.Street[0] : null,
                AddressLine2 = billingAddress.Street.Count > 1 ? billingAddress.Street[1] : null,
                City = billingAddress.City,
                State = !string.IsNullOrEmpty(billingAddress.State) ? billingAddress.State : billingAddress.Country, // fill in mandatory for countries that have no states
                StateDisplayName = billingStateDisplayName,
                KenticoStateID = billingAddress.StateId,
                KenticoCountryID = billingAddress.CountryId,
                AddressCompanyName = settings.DefaultSiteCompanyName,
                isoCountryCode = billingAddress.Country,
                AddressPersonalName = settings.DefaultSitePersonalName,
                Zip = billingAddress.Zip,
                Country = billingAddress.Country,
                KenticoAddressID = 0
            };
        }

        public AddressDTO CreateShippingAddress(DeliveryAddress shippingAddress, Customer customer)
        {
            return new AddressDTO()
            {
                AddressLine1 = shippingAddress.Address1,
                AddressLine2 = shippingAddress.Address2,
                City = shippingAddress.City,
                State = !string.IsNullOrEmpty(shippingAddress.State?.StateCode) ? shippingAddress.State.StateCode : shippingAddress.Country.Name, // fill in mandatory for countries that have no states
                StateDisplayName = shippingAddress.State?.StateDisplayName,
                KenticoStateID = shippingAddress.State?.Id,
                KenticoCountryID = shippingAddress.Country.Id,
                AddressCompanyName = customer.Company,
                isoCountryCode = shippingAddress.Country.Code,
                AddressPersonalName = $"{customer.FirstName} {customer.LastName}",
                Zip = shippingAddress.Zip,
                Country = shippingAddress.Country.Name,
                KenticoAddressID = shippingAddress.Id
            };
        }

        public CustomerDTO CreateCustomer(Customer customer)
        {
            return new CustomerDTO()
            {
                CustomerNumber = customer.CustomerNumber,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                KenticoCustomerID = customer.Id,
                KenticoUserID = customer.UserID,
                Phone = customer.Phone
            };
        }

        public PaymentOptionDTO CreatePaymentOption(Models.PaymentMethod paymentMethod, SubmitOrderRequest request)
        {
            return new PaymentOptionDTO()
            {
                PaymentOptionName = paymentMethod.ShortClassName,
                PONumber = request?.PaymentMethod?.Invoice
            };
        }
    }
}