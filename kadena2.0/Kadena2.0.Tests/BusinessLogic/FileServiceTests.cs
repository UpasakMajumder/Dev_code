using Kadena.BusinessLogic.Services;
using System;
using Xunit;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kadena2.MicroserviceClients.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Common;

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

        [Fact]
        public void ConvertToXlsx_NullData()
        {
            Assert.Throws<ArgumentNullException>(() => Sut.ConvertToXlsx(null));
        }

        [Fact]
        public void ConvertToXlsx_NullRows()
        {
            Assert.Throws<NullReferenceException>(() => Sut.ConvertToXlsx(new TableView { Rows = null }));
        }

        [Fact]
        public void ConvertToXlsx()
        {
            var actualResult = Sut.ConvertToXlsx(new TableView());

            Assert.NotNull(actualResult);
        }
    }
}
