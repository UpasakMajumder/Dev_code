using AutoMapper;
using CMS.Ecommerce;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class KenticoCustomerProvider : IKenticoCustomerProvider
    {
        private readonly IMapper _mapper;

        public KenticoCustomerProvider(IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
            var customers = CustomerInfoProvider.GetCustomers()
                .WhereEquals("CustomerApproverUserID", approverUserId)
                .ToArray();
            return customers
                .Select(c => _mapper.Map<Customer>(c));

        }
    }
}
