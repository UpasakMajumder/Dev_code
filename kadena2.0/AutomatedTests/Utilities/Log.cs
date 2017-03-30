using NUnit.Framework;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace AutomatedTests.Utilities
{
    /// <summary>
    /// Allows to log into console and text file
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Listner for console
        /// </summary>
        private static ConsoleTraceListener ctl { get; set; }
        /// <summary>
        /// Listner for txt file
        /// </summary>
        private static TextWriterTraceListener twtl { get; set; }

        /// <summary>
        /// Fullpath to log file
        /// </summary>
        private static string LogPath { get; set; }

        private static DateTime startTime;

        static Log()
        {

            Trace.Listeners.Clear();

            LogPath = TestEnvironment.TestPath + string.Format(@ConfigurationManager.AppSettings["logdirectory"],
                DateTime.Now.ToString(TestEnvironment.DateFormat));

            if (!Directory.Exists(LogPath))
                Directory.CreateDirectory(LogPath);

            var logName = string.Format(ConfigurationManager.AppSettings["logname"], DateTime.Now.ToString(TestEnvironment.DateTimeFormat));

            LogPath = LogPath + logName;

            twtl = new TextWriterTraceListener(LogPath);
            ctl = new ConsoleTraceListener(false);

            Trace.Listeners.Add(twtl);
            Trace.Listeners.Add(ctl);
        }

        /// <summary>
        /// Closes txt and console tracers so the files can be used 
        /// </summary>
        public static void CloseTracers()
        {
            twtl.Close();
            ctl.Close();
        }

        /// <summary>
        /// Marks start of testing in console and text files
        /// </summary>
        public static void StartOfFixture()
        {
            startTime = DateTime.Now;
            WriteLine("----------------------------------------------------------------------------------------------------------");
            WriteLine("TESTING STARTED");
            WriteLine("Local date and time: " + startTime.ToString(TestEnvironment.ReadableDateTimeFormat));
            WriteLine("Test directory: " + TestEnvironment.TestPath);
            WriteLine("----------------------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// Marks end of tesing in console and text files
        /// </summary>
        public static void EndOfFixture()
        {
            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime - startTime;

            WriteLine("----------------------------------------------------------------------------------------------------------");
            WriteLine("Local time: " + endTime.ToString(TestEnvironment.ReadableDateTimeFormat));
            WriteLine("Testing took: " + duration.ToString("c") + " (" + duration.TotalSeconds + "s)");
            WriteLine("Number of failed tests: " + TestEnvironment.FailCount.ToString());

            if (TestEnvironment.FailedTests.Count > 0)
            {
                WriteLine("Failed tests:");

                foreach (var test in TestEnvironment.FailedTests)
                {
                    WriteLine("\t{0}", test);
                }
            }

            WriteLine("----------------------------------------------------------------------------------------------------------");
        }

        /// <summary>
        /// Helps trace start of test in logs
        /// </summary>
        public static void StartOfTest()
        {
            WriteLine("START [{0}] - {1}", new object[] { TestEnvironment.TestName, DateTime.Now.ToString(TestEnvironment.ReadableDateTimeFormat) });
            for (int i = 0; i < 3; i++)
            {

                WriteLine(".");
            }
        }

        /// <summary>
        /// Helps trace end of test in logs
        /// </summary>
        public static void EndOfTest()
        {
            for (int i = 0; i < 3; i++)
            {
                WriteLine(".");
            }
            WriteLine("END - Test {0} - {1}", new object[] { TestEnvironment.TestName, DateTime.Now.ToString(TestEnvironment.ReadableDateTimeFormat) });
            if (TestEnvironment.IsFailed)
                WriteLine("Error message: {0}\r\n", TestContext.CurrentContext.Result.Message);
        }

        /// <summary>
        /// Writes line of text into added listners
        /// </summary>
        public static void WriteLine(string s)
        {
            Trace.WriteLine(s);
            Trace.Flush();
        }

        /// <summary>
        /// Writes line of text into added listners
        /// </summary>
        /// <param name="text">Unformated text</param>
        /// <param name="arg">Argument to be formated with text</param>
        public static void WriteLine(string text, object arg)
        {
            WriteLine(string.Format(text, arg));
        }

        /// <summary>
        /// Writes line of text into added listners
        /// </summary>
        /// <param name="text">Unformated text</param>
        /// <param name="arg">Array of arguments to be formated with text</param>
        public static void WriteLine(string text, params object[] arg)
        {
            WriteLine(string.Format(text, arg));
        }
    }
}
