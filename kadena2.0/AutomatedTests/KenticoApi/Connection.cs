using AutomatedTests.Utilities;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.KenticoApi
{
    class Connection
    {
        readonly string login;
        readonly string password;
        
        public Connection(string login, string password)
        {
            this.login = login;
            this.password = password;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(TestEnvironment.Url + "/rest");
            client.Authenticator = new HttpBasicAuthenticator(login, password);
            client.AddDefaultParameter("format", "json", ParameterType.QueryString);

            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response from Kentico API.";
                throw new ApplicationException(message, response.ErrorException);
            }
            return response.Data;
        }
    }
}
