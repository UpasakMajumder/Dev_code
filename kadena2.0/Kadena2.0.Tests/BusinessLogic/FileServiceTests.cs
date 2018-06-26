using Kadena.BusinessLogic.Services;
using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.AmazonFileSystemProvider;
using System.Threading.Tasks;
using Kadena.Dto.General;

namespace Kadena.Tests.BusinessLogic
{
    public class FileServiceTests : KadenaUnitTest<FileService>
    {
        [Theory(DisplayName = "FileService()")]
        [ClassData(typeof(FileServiceTests))]
        public void FileService(IFileClient fileClient, IKenticoLogger logger, IS3PathService pathService)
        {
            Assert.Throws<ArgumentNullException>(() => new FileService(fileClient, logger, pathService));
        }

        
    }
}
