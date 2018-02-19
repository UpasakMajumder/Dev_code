using Kadena.Dto.Product;
using Kadena.WebAPI.Controllers;
using Kadena.WebAPI.Infrastructure.Communication;
using Moq;
using Moq.AutoMock;
using System.Collections.Generic;
using System.Web.Http.Results;
using Xunit;

namespace Kadena.Tests.WebApi
{
    public class ProductsControllerTest
    {
        [Fact(DisplayName = "ProductsContoller.GetPrice()")]
        public void GetPrice()
        {
            var autoMocker = new AutoMocker();
            var controller = autoMocker.CreateInstance<ProductsController>();

            var actionResult = controller.GetPrice(0, new Dictionary<string, int>());

            Assert.IsType<JsonResult<BaseResponse<PriceDto>>>(actionResult);
            Assert.NotNull(actionResult);
        }
    }
}
