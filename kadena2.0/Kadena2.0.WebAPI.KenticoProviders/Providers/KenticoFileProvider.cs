using Kadena.WebAPI.KenticoProviders.Contracts;
using CMS.IO;
using CMS.SiteProvider;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class KenticoFileProvider : IKenticoFileProvider
    {
        public void CreateFile(string filePath, System.IO.Stream fileStream)
        {
            using (var sw = File.Create(filePath))
            {
                fileStream.Seek(0, System.IO.SeekOrigin.Begin);
                fileStream.CopyTo(sw);
            }
        }

        public string GetFileUrl(string path)
        {
            if (!path.StartsWith("~"))
            {
                path = $"~/{path.TrimStart('/')}";
            }
            return File.GetFileUrl(path, SiteContext.CurrentSiteName);
        }
    }
}
