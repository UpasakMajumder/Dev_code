using Kadena.Models.Program;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoProgramsProvider
    {
        void DeleteProgram(int programID);

        List<int> GetProgramIDsByCampaign(int campaignID);

        CampaignProgram GetProgram(int programID);
    }
}
