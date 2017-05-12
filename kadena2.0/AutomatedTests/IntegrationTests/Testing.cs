using NUnit.Framework;
using AutomatedTests.KenticoApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomatedTests.KenticoApi.Objects;

namespace AutomatedTests.IntegrationTests
{
    class Testing : BaseIntegrationTest
    {
        [Test]
        public void ApiTest()
        {
            var category = new ProductCategory().Init();
            var categoryResponse = Api.InsertDocument("/Products", category);
            var product = new Product().Init();
            var productResponse = Api.InsertDocument($"{categoryResponse.NodeAliasPath}", product);

            var deleteResponse = Api.DeleteDocument<ProductCategory>($"{categoryResponse.NodeAliasPath}");
        }
    }
}
