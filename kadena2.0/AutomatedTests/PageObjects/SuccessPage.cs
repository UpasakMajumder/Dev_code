﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.PageObjects
{
    class SuccessPage : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = ".submitted")]
        private IWebElement SubmitConfirmation { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".submitted svg use")]
        private IWebElement InformationPicture { get; set; }

        public SuccessPage()
        {
            PageFactory.InitElements(Browser.Driver, this);
        }

        public bool IsSubmitConfirmationDisplayed()
        {
            SubmitConfirmation.WaitTillVisible();
            return SubmitConfirmation.IsDisplayed();
        }

        /// <summary>
        /// Verifies if the picture on the page indicates success of order submission
        /// </summary>
        /// <returns></returns>
        public bool IsSuccessPictureDisplayed()
        {
            SubmitConfirmation.WaitTillVisible();
            return InformationPicture.GetAttribute("xlink:href") == "/gfx/svg/sprites/icons.svg#order-ready";
        }
    }
}