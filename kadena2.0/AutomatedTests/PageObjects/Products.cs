using AutomatedTests.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutomatedTests.PageObjects
{
    class Products : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = "h1>a")]
        private IWebElement RequestNewProductBtn { get; set; }



        public Products()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public void Open()
        {
            Browser.Driver.Navigate().GoToUrl($"{TestEnvironment.Url}/products");
        }

        public ProductRequestForm ClickRequestNewProduct()
        {
            RequestNewProductBtn.ClickElement();
            return new ProductRequestForm();
        }

       



    }
}
