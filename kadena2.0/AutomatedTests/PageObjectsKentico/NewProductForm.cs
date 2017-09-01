using AutomatedTests.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjectsKentico
{
    class NewProductForm : BasePage
    {
        [FindsBy(How = How.Id, Using = "m_c_layoutElem_contentview")]
        private IWebElement ContentViewFrame { get; set; }

        [FindsBy(How = How.Id, Using = "m_c_productEditElem_g_SKUName_txtText")]
        private IWebElement ProductName { get; set; }

        [FindsBy(How = How.Id, Using = "m_c_productEditElem_g_SKUNumber_txtText")]
        private IWebElement SkuNumber { get; set; }

        [FindsBy(How = How.Id, Using = "m_c_productEditElem_g_SKUPrice_txtPrice")]
        private IWebElement Price { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#m_c_productEditElem_f_ProductType_list>label")]
        private IList<IWebElement> ProductTypesCheckboxes { get; set; }

        [FindsBy(How = How.CssSelector, Using = "[value=Save]")]
        private IWebElement SaveButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#field_SKUImagePath input")]
        private IWebElement UploadImageBtn { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#field_SKUWeight input")]
        private IWebElement Weight { get; set; }

        public NewProductForm()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public enum ProductTypes
        {
            Static,
            Inventory,
            POD,
            Mailing,
            Templated,
            WithAddons
        }
        
        public void SwitchToContentViewFrame()
        {
            ContentViewFrame.SwitchToIframe();
        }

        /// <summary>
        /// Fills out fields in the product form
        /// </summary>
        /// <param name="productName"></param>
        public void FillOutFieldsForStatic(string productName)
        {
            ProductName.EnterText(productName);
            SkuNumber.EnterText(Lorem.RandomNumber(10000,99999).ToString());
            Price.EnterText(Lorem.RandomNumber(100, 999).ToString());            
            ProductTypesCheckboxes[(int)ProductTypes.Static].ClickElement();
            SelectImage();
            Weight.EnterText("0.1");
        }

        /// <summary>
        /// Clicks Save button
        /// </summary>
        public void SaveForm()
        {
            SaveButton.ClickElement();
        }

        /// <summary>
        /// Sends path to image to the image input field
        /// </summary>
        public void SelectImage()
        {
            string path = TestEnvironment.TestPath + "\\TestFiles\\testpicture.jpg";
            UploadImageBtn.SendText(path);
        }
    }
}
