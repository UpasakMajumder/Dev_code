using AutoMapper;
using CMS.Ecommerce;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using Kadena.Models.SiteSettings;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class KenticoCustomerProvider : IKenticoCustomerProvider
    {
        private readonly IMapper _mapper;
        private readonly IKenticoResourceService _resources;

        public KenticoCustomerProvider(IMapper mapper, IKenticoResourceService resources)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public Customer GetCurrentCustomer()
        {
            return _mapper.Map<Customer>(ECommerceContext.CurrentCustomer);
        }

        public Customer GetCustomer(int customerId)
        {
            return _mapper.Map<Customer>(CustomerInfoProvider.GetCustomerInfo(customerId));
        }

        public Customer GetCustomerByUser(int userId)
        {
            return _mapper.Map<Customer>(CustomerInfoProvider.GetCustomerInfoByUserID(userId));
        }
        
        public int CreateCustomer(Customer customer)
        {
            var customerInfo = _mapper.Map<CustomerInfo>(customer);
            customerInfo.CustomerID = 0;
            customerInfo.Insert();
            return customerInfo.CustomerID;
        }

        public void UpdateCustomer(Customer customer)
        {
            var customerInfo = CustomerInfoProvider.GetCustomerInfo(customer.Id);

            if (customerInfo == null)
            {
                throw new ArgumentOutOfRangeException(nameof(customer.Id), "Existing Customer with given Id not found");
            }

            customerInfo.CustomerFirstName = customer.FirstName;
            customerInfo.CustomerLastName = customer.LastName;
            customerInfo.CustomerEmail = customer.Email;
            customerInfo.CustomerPhone = customer.Phone;
            customerInfo.CustomerCompany = customer.Company;
            customerInfo.Update();
        }

        public IEnumerable<Customer> GetCustomersByApprover(int approverUserId)
        {
            // retrieve t the customer that have the approver user set 
            var customers = CustomerInfoProvider.GetCustomers()
                .WhereEquals("CustomerApproverUserID", approverUserId).ToList();

            // retrieve the customer that have the approver user is not set 
            var customersNoApproverAssigned = CustomerInfoProvider.GetCustomers()
                    .WhereEquals("CustomerApproverUserID", 0).ToList();
            /* 
             * If customers are found with no approval user  retrieve the default approval user for the site from the settings 
             * Compare it with the passed in user and if they are the same union the customers and sent it across 
             * If not jsut send the customers where the approver user matched the passed in user  
            */

            if (customersNoApproverAssigned.Any())
            {
                var defaultApproverId = _resources.GetSiteSettingsKey(Settings.KDA_DefaultOrderApprover);
                if (!string.IsNullOrEmpty(defaultApproverId))
                {
                    if (approverUserId.ToString() == defaultApproverId)
                    {
                        customers.AddRange(customersNoApproverAssigned);
                    }
                }
            }

            return customers.Select(c => _mapper.Map<Customer>(c));

        }

        public IEnumerable<Customer> GetCustomersByManufacturerID(int manufacturerID)
        {
            var customers = CustomerInfoProvider.GetCustomers()
                .WhereEquals("CustomerManufacturerID", manufacturerID)
                .ToArray();
            return customers
                .Select(c => _mapper.Map<Customer>(c));

        }
    }
}
