using OpenQA.Selenium;
using System;
using System.Configuration;
using System.IO;

namespace AutomatedTests.Utilities
{
    /// <summary>
    /// Allows to take screenshots
    /// </summary>
    public class Screenshot
    {
        private static string ShotPath { get; set; }
        private static string ShotName
        {
            get
            {
                return string.Format(@ConfigurationManager.AppSettings["screenshotname"],
                    TestEnvironment.TestName, DateTime.Now.ToString(TestEnvironment.DateTimeFormat));
            }
        }

        static Screenshot()
        {
            ShotPath = TestEnvironment.TestPath + string.Format(@ConfigurationManager.AppSettings["screenshotdirectory"],
                DateTime.Now.ToString(TestEnvironment.DateFormat));

            if (!Directory.Exists(ShotPath))
                Directory.CreateDirectory(ShotPath);
        }

        /// <summary>
        /// Takes screenshot and saves it in desired location
        /// </summary>
        /// <param name="format">Format of image file</param>
        public static void TakeScreenshot(ScreenshotImageFormat format = ScreenshotImageFormat.Png)
        {
            OpenQA.Selenium.Screenshot shot = ((ITakesScreenshot)Browser.Driver).GetScreenshot();
            shot.SaveAsFile(ShotPath + ShotName, format);
        }
    }
}
