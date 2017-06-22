using System.Data;

namespace Kadena.WebAPI.Contracts
{
    public interface IKenticoSearchService
    {
        DataSet Search(string phrase, string indexName, string path, bool checkPermissions);
    }
}