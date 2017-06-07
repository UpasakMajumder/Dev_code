using AutomatedTests.KenticoApi;
using AutomatedTests.KenticoApi.Objects;
using AutomatedTests.PageObjects;
using AutomatedTests.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.Tests
{
    class CheckoutProcess : BaseTest
    {
        [Test]
        public void When_ProductInCart_Expect_ShippingCostsEstimated()
        {
            //Create a category using Kentico API
            var category = new ProductCategory().Init();
            var categoryResponse = Api.InsertDocument("/Products", category);

            //Insert a product using Kentico API
            var product = new Product().Init();
            var productResponse = Api.InsertDocument($"{categoryResponse.NodeAliasPath}", product);

            //Login to Kadena
            var login = new Login();
            login.Open();
            login.FillLogin(TestCustomer.Name, TestCustomer.Password);
            var dashboard = login.Submit();
            dashboard.WaitForKadenaPageLoad();

            //Go to the product you created and add it to cart
            ProductDetail productDetail = new ProductDetail();
            productDetail.Open(category.ProductCategoryTitle, product.DocumentName);
            productDetail.ClickAddToCart();

            //Go to checkout and verify if shipping cost is estimated
            Checkout checkout = new Checkout();
            checkout.Open();
            checkout.SelectAddress(1);
            Assert.IsTrue(checkout.AreShippingCostEstimated(), "Shipping Cost is not estimated");
            checkout.SelectEstimatedCarrier();

            //verify if total and subtotal numbers are correct
            Assert.IsTrue(checkout.IsSubTotalCorrect());
            Assert.IsTrue(checkout.isTotalCorrect());

            //Delete the category with the product using Kentico Api
            var deleteResponse = Api.DeleteDocument<ProductCategory>($"{categoryResponse.NodeAliasPath}");
        }
    }
}

