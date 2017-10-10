using AutomatedTests.PageObjects;
using AutomatedTests.PageObjectsKentico;
using AutomatedTests.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutomatedTests.PageObjectsKentico.BasePageComponents.ApplicationsMenu;
using AutomatedTests.Domain.Constants;
using AutomatedTests.KenticoApi.Objects;
using AutomatedTests.KenticoApi;

namespace AutomatedTests.Tests
{
    class ProductsTests : BaseTest
    {
        [Test]
        public void When_UserWantsToRequestNewProduct_Expect_FormIsSubmitted()
        {
            //Login to Kadena
            InitializeTest();
            var dashboard = new Dashboard();

            //Navigate to products and click request new product
            Products products = new Products();
            products.Open();
            ProductRequestForm productRequestForm = products.ClickRequestNewProduct();

            //try to submit empty form
            productRequestForm.SubmitForm();
            Assert.IsTrue(productRequestForm.IsDescriptionValidationDisplayed());

            //fill out and submit the form
            productRequestForm.SelectAttachment();
            productRequestForm.FillOutDescription();
            productRequestForm.SubmitForm();

            SuccessPage formSuccess = new SuccessPage();
            Assert.IsTrue(formSuccess.IsSubmitConfirmationDisplayed());
        }

        [Test]
        public void When_NewProductCreated_Expect_ProductIsCreated()
        {
            var dashboard = InitializeAdminTest();

            var adminHome = new AdminHomePage();
            adminHome.Open();

            //Open Pages Application
            adminHome.ClickApplistIcon();
            adminHome.applicationsMenu.SelectSubcategory(ApplicationCategoriesOptions.ContentManagement);
            adminHome.applicationsMenu.OpenApplication(KenticoApplicationsNames.Pages);

            //Create new product
            var contentTree = new PagesContentTree();
            contentTree.SelectActionFromContextMenu(PagesOptionsNames.New, PagesOptionsNames.Products);
            var selectNewPageType = new SelectNewPageType();
            selectNewPageType.SelectPageType(SelectNewPageType.PageTypeOptions.Product);

            //Fill out necessary fields
            var newProductForm = new NewProductForm();
            string productName = "automation" + StringHelper.RandomString(5);
            newProductForm.FillOutFieldsForStatic(productName);
            newProductForm.SaveForm();
            newProductForm.LogoutFromKentico();
            EndAdminTest();

            //Verify if product is published on the website
            var login = new Login();
            login.Open();
            dashboard = login.LoginAndSubmit(TestCustomer.Name, TestCustomer.Password);
            dashboard.WaitForRecentOrders();
            ProductDetail productDetail = new ProductDetail();
            productDetail.Open(productName);
            Assert.IsTrue(productDetail.IsProductImageThumbnailDisplayed());

            //Delete the product
            var deleteResponse = Api.DeleteDocument<Product>("/Products/" + productName);
        }

        [Test]
        public void When_StaticProductCreated_Expect_YouCanSubmitOrder()
        {
            var dashboard = InitializeAdminTest();

            var adminHome = new AdminHomePage();
            adminHome.Open();

            //Open Pages Application
            adminHome.ClickApplistIcon();
            adminHome.applicationsMenu.SelectSubcategory(ApplicationCategoriesOptions.ContentManagement);
            adminHome.applicationsMenu.OpenApplication(KenticoApplicationsNames.Pages);

            //Create new product
            var contentTree = new PagesContentTree();
            contentTree.SelectActionFromContextMenu(PagesOptionsNames.New, PagesOptionsNames.Products);
            var selectNewPageType = new SelectNewPageType();
            selectNewPageType.SelectPageType(SelectNewPageType.PageTypeOptions.Product);

            //Fill out necessary fields
            var newProductForm = new NewProductForm();
            string productName = "automation" + StringHelper.RandomString(5) + "static";
            newProductForm.FillOutFieldsForStatic(productName);
            newProductForm.SaveForm();
            newProductForm.LogoutFromKentico();
            EndAdminTest();

            //Verify if product is published on the website
            var login = new Login();
            login.Open();
            dashboard = login.LoginAndSubmit(TestCustomer.Name, TestCustomer.Password);
            dashboard.WaitForRecentOrders();

            //empty the car
            var checkout = new Checkout();
            checkout.Open();
            checkout.EmptyTheCart();

            //add the product to cart and place order
            ProductDetail productDetail = new ProductDetail();
            productDetail.Open(productName);
            Assert.IsTrue(productDetail.IsProductImageThumbnailDisplayed());
            checkout = productDetail.AddProductToCart();
            checkout.FillOutPurchaseOrderNumber();
            checkout.PlaceOrder();

            //check if the order was successfully placed
            var successPage = new SuccessPage();
            Assert.IsTrue(successPage.IsSuccessPictureDisplayed());
            string orderID = successPage.GetOrderID();

            //check if the order is in recent orders and view detail
            var recentOrders = new RecentOrders();
            recentOrders.Open();
            recentOrders.WaitForTable();
            Assert.IsTrue(recentOrders.IsTheOrderDisplayedInTable(orderID));
            var orderDetail = new OrderDetail();
            orderDetail.Open(orderID);

            //Delete the product
            var deleteResponse = Api.DeleteDocument<Product>("/Products/" + productName);
        }
    }
}

