using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjectsKentico.BasePageComponents
{
    class ApplicationsMenu
    {
        [FindsBy(How = How.CssSelector, Using = "h3>span")]
        private IList<IWebElement> ApplicationsCategories { get; set; }

        [FindsBy(How = How.CssSelector, Using = "li>a")]
        private IList<IWebElement> Applications { get; set; }

        public ApplicationsMenu()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public enum ApplicationCategoriesOptions
        {
            ContentManagement = 0,
            OnLineMarketing = 1,
            Ecommerce = 2,
            SocialAndCommunity = 3,
            Development = 4,
            Configuration = 5
        }

        /// <summary>
        /// Selects subcategory
        /// </summary>
        /// <param name="category">Category from predefined options</param>
        public void SelectSubcategory(ApplicationCategoriesOptions category)
        {
            ApplicationsCategories[(int)category].ClickElement();
        }

        /// <summary>
        /// Opens application
        /// </summary>
        /// <param name="appName">name of application</param>
        public void OpenApplication(string appName)
        {
            IList<IWebElement> displayedApplications = Applications.Where(r => r.IsDisplayed()).ToList();
            displayedApplications.Single(r => r.GetText() == appName).ClickElement();
        }
    }
}
