using System.Collections.Generic;
using System.Data;

namespace Kadena.WebAPI.Contracts
{
    public interface IKenticoSearchService
    {
        IEnumerable<DataRow> Search(string phrase, string indexName, string path, int results, bool checkPermissions);
    }
}