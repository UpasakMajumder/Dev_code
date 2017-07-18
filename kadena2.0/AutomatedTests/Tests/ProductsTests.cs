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
    class ProductsTests : BaseTest
    {
        [Test]
        public void When_UserWantsToRequestNewProduct_Expect_FormIsSubmitted()
        {
            //Login to Kadena
            var dashboard = InitializeTest();

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

            FormSuccess formSuccess = new FormSuccess();
            Assert.IsTrue(formSuccess.IsSubmitConfirmationDisplayed());
        }
    }
}
