using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class ProgramsService : IProgramsService
    {
        private readonly IKenticoProgramsProvider kenticoProgram;

        public ProgramsService(IKenticoProgramsProvider kenticoProgram)
        {
            this.kenticoProgram = kenticoProgram;
        }

        public void DeleteProgram(int programID)
        {
            kenticoProgram.DeleteProgram(programID);
        }
    }
}
