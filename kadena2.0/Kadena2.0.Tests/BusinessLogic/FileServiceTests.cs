using Kadena.BusinessLogic.Services;
using System;
using Xunit;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.Tests.BusinessLogic
{
    public class FileServiceTests : KadenaUnitTest<FileService>
    {
        [Theory]
        [ClassData(typeof(FileServiceTests))]
        public void FileService(IFileClient fileClient, IKenticoResourceService resources, IKenticoLogger logger)
        {
            Assert.Throws<ArgumentNullException>(() => new FileService(fileClient, resources, logger));
        }
    }
}
