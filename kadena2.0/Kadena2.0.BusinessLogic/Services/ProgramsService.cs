using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class ProgramsService : IProgramsService
    {
        private readonly IKenticoProgramsProvider kenticoProgram;

        public ProgramsService(IKenticoProgramsProvider kenticoProgram)
        {
            if (kenticoProgram == null)
            {
                throw new ArgumentNullException(nameof(kenticoProgram));
            }
            this.kenticoProgram = kenticoProgram;
        }

        public void DeleteProgram(int programID)
        {
            kenticoProgram.DeleteProgram(programID);
        }
    }
}
