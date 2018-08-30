using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Kadena.Models;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IKListService
    {
        Task<bool> UseOnlyCorrectAddresses(Guid containerId);

        Task<bool> UpdateAddresses(Guid containerId, IEnumerable<MailingAddress> addresses);

        Task<MailingList> GetMailingList(Guid containerId);

        Task<string> DeleteExpiredMailingLists();

        string CreateMailingList(string fileName, Stream fileStream);

        Task<Uri> GetContainerFileUrl(Guid containerId);
    }
}
