using Kadena.BusinessLogic.Services;
using Kadena.Models.Common;
using System;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class XlsxConvertTests : KadenaUnitTest<XlsxConvert>
    {
        [Fact]
        public void ConvertToXlsx_NullData()
        {
            Assert.Throws<ArgumentNullException>(() => Sut.GetBytes(null));
        }

        [Fact]
        public void ConvertToXlsx_NullRows()
        {
            Assert.Throws<NullReferenceException>(() => Sut.GetBytes(new TableView { Rows = null }));
        }

        [Fact]
        public void ConvertToXlsx()
        {
            var actualResult = Sut.GetBytes(new TableView());

            Assert.NotNull(actualResult);
        }
    }
}
