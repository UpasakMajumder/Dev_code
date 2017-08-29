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
            newProductForm.FillOutFields(productName);
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
    }
}

