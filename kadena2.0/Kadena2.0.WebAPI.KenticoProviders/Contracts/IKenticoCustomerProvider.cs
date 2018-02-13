using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
   public interface IKenticoCustomerProvider
    {
        int GetUserIDByCustomerID(int customerID);
        int GetCustomerIDByUserID(int userID);
    }
}
