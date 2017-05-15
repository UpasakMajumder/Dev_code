using AutomatedTests.KenticoApi.Objects;
using AutomatedTests.Utilities;
using RestSharp;

namespace AutomatedTests.KenticoApi
{
    public static class Api
    {
        public enum WorkflowAction
        {
            publish,
            checkout,
            checkin,
            archive,
            movetonextstep,
            movetopreviousstep
        }

        private static Connection api;

        static Api()
        {
            api = new Connection(TestUser.Name, TestUser.Password);
        }


        /// <summary>
        /// Gets single document on provided path
        /// </summary>
        /// <typeparam name="T">Page type</typeparam>
        /// <param name="path">Path to document</param>
        /// <param name="site">Website containing document</param>
        /// <param name="culture">Required culture</param>
        /// <returns>Document on path</returns>
        public static T GetDocument<T>(string path, string site = "currentsite", string culture = "en-us") where T : new()
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "/content/{site}/{culture}/document/{path}";
            request.AddUrlSegment("site", site);
            request.AddUrlSegment("culture", culture);
            request.AddUrlSegment("path", path);
            return api.Execute<T>(request);
        }

        /// <summary>
        /// Gets all pages starting with provided path
        /// </summary>
        /// <typeparam name="T">Page type</typeparam>
        /// <param name="path">Path to document</param>
        /// <param name="site">Website containing document</param>
        /// <param name="culture">Required culture</param>
        /// <returns>Document on path</returns>
        public static T GetDocuments<T>(string path, string site = "currentsite", string culture = "en-us") where T : new()
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "/content/{site}/{culture}/all/{path}";
            request.AddUrlSegment("site", site);
            request.AddUrlSegment("culture", culture);
            request.AddUrlSegment("path", path);
            return api.Execute<T>(request);
        }

        /// <summary>
        /// Gets all chiled pages of provided document
        /// </summary>
        /// <typeparam name="T">Page type</typeparam>
        /// <param name="path">Path to document</param>
        /// <param name="site">Website containing document</param>
        /// <param name="culture">Required culture</param>
        /// <returns>Document on path</returns>
        public static T GetChildrensOf<T>(string path, string site = "currentsite", string culture = "en-us") where T : new()
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "/content/{site}/{culture}/all/{path}";
            request.AddUrlSegment("site", site);
            request.AddUrlSegment("culture", culture);
            request.AddUrlSegment("path", path);
            return api.Execute<T>(request);
        }

        /// <summary>
        /// Inserts page on given path
        /// </summary>
        /// <typeparam name="T">Page type</typeparam>
        /// <param name="path">Path to document</param>
        /// <param name="site">Website containing document</param>
        /// <param name="culture">Required culture</param>
        /// <returns>Document on path</returns>
        public static T InsertDocument<T>(string path, T document, string site = "currentsite", string culture = "en-us") where T : new()
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "/content/{site}/{culture}/document/{path}";
            request.AddUrlSegment("site", site);
            request.AddUrlSegment("culture", culture);
            request.AddUrlSegment("path", path);
            request.AddJsonBody(document);
            return api.Execute<T>(request);
        }


        /// <summary>
        /// Update page on given path
        /// </summary>
        /// <typeparam name="T">Page type</typeparam>
        /// <param name="path">Path to document</param>
        /// <param name="site">Website containing document</param>
        /// <param name="culture">Required culture</param>
        /// <returns>Document on path</returns>
        public static T UpdateDocument<T>(string path, T document, string site = "currentsite", string culture = "en-us") where T : new()
        {
            var request = new RestRequest(Method.PUT);
            request.Resource = "/content/{site}/{culture}/document/{path}";
            request.AddUrlSegment("site", site);
            request.AddUrlSegment("culture", culture);
            request.AddUrlSegment("path", path);
            request.AddJsonBody(document);
            return api.Execute<T>(request);
        }

        /// <summary>
        /// Changes workflow of document
        /// </summary>
        /// <typeparam name="T">Page type</typeparam>
        /// <param name="path">Path to document</param>
        /// <param name="action">Workflow action</param>
        /// <param name="site">Website containing document</param>
        /// <param name="culture">Required culture</param>
        /// <returns></returns>
        public static T ChangeWorkflow<T>(string path, WorkflowAction action, string site = "currentsite", string culture = "en-us") where T : new()
        {
            var request = new RestRequest(Method.PUT);
            request.Resource = "/content/{site}/{culture}/{workflow}/{path}";
            request.AddUrlSegment("site", site);
            request.AddUrlSegment("culture", culture);
            request.AddUrlSegment("workflow", action.ToString());
            request.AddUrlSegment("path", path);
            return api.Execute<T>(request);
        }

        /// <summary>
        /// Update page on given path
        /// </summary>
        /// <typeparam name="T">Page type</typeparam>
        /// <param name="path">Path to document</param>
        /// <param name="site">Website containing document</param>
        /// <param name="culture">Required culture</param>
        /// <returns>Empty document</returns>
        public static T DeleteDocument<T>(string path, string site = "currentsite", string culture = "en-us") where T : new()
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "/content/{site}/{culture}/document/{path}";
            request.AddUrlSegment("site", site);
            request.AddUrlSegment("culture", culture);
            request.AddUrlSegment("path", path);
            return api.Execute<T>(request);
        }
    }
}
