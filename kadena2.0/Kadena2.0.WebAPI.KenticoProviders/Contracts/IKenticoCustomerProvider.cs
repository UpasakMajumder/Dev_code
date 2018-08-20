using Kadena.Models;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
   public interface IKenticoCustomerProvider
    {
        Customer GetCurrentCustomer();
        Customer GetCustomer(int customerId);
        Customer GetCustomerByUser(int userId);

        /// <summary>
        /// Creates and saves new Customer
        /// </summary>
        /// <returns>ID of new Customer</returns>
        int CreateCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        IEnumerable<Customer> GetCustomersByApprover(int approverUserId);
        IEnumerable<Customer> GetCustomersByManufacturerID(int manufacturerID);
    }
}
