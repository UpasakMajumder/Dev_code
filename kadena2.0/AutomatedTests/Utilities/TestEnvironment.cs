using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Configuration;

namespace AutomatedTests.Utilities
{
    public enum Env
    {
        Dev,
        Test,
        Stage
    }
    /// <summary>
    /// Context of environment and test
    /// </summary>
    public class TestEnvironment
    {
        /// <summary>
        /// Name of environment
        /// </summary>
        public static Env Name { get; set; }
        /// <summary>
        /// Url to set environment
        /// </summary>
        public static string Url { get; set; }
        /// <summary>
        /// Gets directory to tests (usualy same as dll directory)
        /// </summary>
        public static string TestPath { get; } = TestContext.CurrentContext.TestDirectory;
        /// <summary>
        /// Gets number of failed tests
        /// </summary>
        public static int FailCount { get { return TestContext.CurrentContext.Result.FailCount; } }
        /// <summary>
        /// Gets name of current test
        /// </summary>
        public static string TestName { get { return TestContext.CurrentContext.Test.Name; } }
        /// <summary>
        /// List of failed test names, needs to run IsTestFailed() or SaveFailedTest()
        /// </summary>
        public static List<string> FailedTests { get; set; }
        /// <summary>
        /// True if test failed
        /// </summary>
        public static bool IsFailed { get { return TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed; } }
        /// <summary>
        /// Holds universal string representing date and time format
        /// </summary>
        public static string DateTimeFormat { get; set; }
        /// <summary>
        /// Holds universal string representing date format
        /// </summary>
        public static string DateFormat { get; set; }
        /// <summary>
        /// Holds universal string representing readable form of date format
        /// </summary>
        public static string ReadableDateTimeFormat { get; set; }

        static TestEnvironment()
        {
            FailedTests = new List<string>();
            DateTimeFormat = ConfigurationManager.AppSettings["datetimeformat"];
            DateFormat = ConfigurationManager.AppSettings["dateformat"];
            ReadableDateTimeFormat = ConfigurationManager.AppSettings["readabledateformat"];
            Name = EnumHelper.Parse<Env>(ConfigurationManager.AppSettings["environment"]);
            Url = ConfigurationManager.AppSettings[Name.ToString()];
        }

        /// <summary>
        /// Gets value representing if the test failed, runs SaveFailedTest() if failed
        /// </summary>
        /// <returns>True if test failed</returns>
        public static bool IsTestFailed()
        {
            var failed = IsFailed;

            if (failed)
                SaveFailedTest();

            return failed;
        }

        /// <summary>
        /// Saves test name into FailedTests list
        /// </summary>
        public static void SaveFailedTest()
        {
            FailedTests.Add(TestContext.CurrentContext.Test.Name);
        }

    }
}
