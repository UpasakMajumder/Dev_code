using Kadena.Models.IBTF;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoIBTFProvider
    {
        void InsertIBTFAdjustmentRecord(IBTFAdjustment inboundAdjustment);

        List<IBTFAdjustment> GetIBTFAdjustmentRecords();

        List<IBTF> GetIBTFRecords();
    }
}