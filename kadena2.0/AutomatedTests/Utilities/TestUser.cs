using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.Utilities
{
    class TestUser
    {
        /// <summary>
        /// User name for environment
        /// </summary>
        public static string Name { get; set; }
        /// <summary>
        /// User password for enveronment
        /// </summary>
        public static string Password { get; set; }

        static TestUser()
        {
            Name = ConfigurationManager.AppSettings["username"];
            Password = ConfigurationManager.AppSettings["password"];
        }

    }
}
