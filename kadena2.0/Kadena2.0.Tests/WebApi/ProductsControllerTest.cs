using Kadena.Dto.Product;
using Kadena.WebAPI.Controllers;
using Kadena.WebAPI.Infrastructure.Communication;
using System.Collections.Generic;
using System.Web.Http.Results;
using Xunit;

namespace Kadena.Tests.WebApi
{
    public class ProductsControllerTest : KadenaUnitTest<ProductsController>
    {
        [Fact(DisplayName = "ProductsContoller.GetPrice()")]
        public void GetPrice()
        {
            var actualResult = Sut.GetPrice(0, new Dictionary<string, int>());

            Assert.IsType<JsonResult<BaseResponse<PriceDto>>>(actualResult);
            Assert.NotNull(actualResult);
        }
    }
}
