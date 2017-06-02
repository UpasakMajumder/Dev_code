using Kadena.WebAPI.Contracts;
using CMS.Helpers;

namespace Kadena.WebAPI.Services
{
    public class KenticoResourceStringService : IResourceStringService
    {
        public string GetResourceString(string name)
        {
            return ResHelper.GetString(name, useDefaultCulture:true);
        }
    }
}