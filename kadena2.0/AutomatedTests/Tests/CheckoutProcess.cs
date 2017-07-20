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
            var dashboard = InitializeTest();
            dashboard.Open();
            dashboard.WaitForRecentOrders();

            //make sure there is nothing in cart
            Checkout checkout = new Checkout();
            checkout.Open();
            checkout.EmptyTheCart();

            //find a product you can add to cart and add it
            dashboard.Open();
            dashboard.WaitForRecentOrders();
            var productDetail = dashboard.SelectProductYouCanAddToCart();

            //Go to checkout and verify if shipping cost is estimated
            checkout.Open();
            checkout.SelectAddress(1);
            Assert.IsTrue(checkout.AreShippingCostEstimated(), "Shipping Cost is not estimated");
            checkout.SelectEstimatedCarrier();

            //verify if tax is estimated
            Assert.IsTrue(checkout.IsTaxEstimated());

            //verify if total and subtotal numbers are correct
            Assert.IsTrue(checkout.IsSubTotalCorrect());
            Assert.IsTrue(checkout.isTotalCorrect());
        }

        [Test]
        public void When_PlacingAnOrder_Expect_OrderIsSubmitted()
        {
            var dashboard = InitializeTest();
            dashboard.Open();
            dashboard.WaitForRecentOrders();

            //make sure there is nothing in cart
            Checkout checkout = new Checkout();
            checkout.Open();
            checkout.EmptyTheCart();

            //find a product you can add to cart and add it
            dashboard.Open();
            dashboard.WaitForRecentOrders();
            var productDetail = dashboard.SelectProductYouCanAddToCart();

            //Go to checkout and place the order
            checkout.Open();            
            checkout.FillOutPurchaseOrderNumber();
            checkout.PlaceOrder();

            //check if the order was successfully placed
            var successPage = new SuccessPage();
            Assert.IsTrue(successPage.IsSuccessPictureDisplayed());
        }
    }
}

