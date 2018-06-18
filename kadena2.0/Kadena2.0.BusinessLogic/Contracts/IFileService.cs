using Kadena.Models.Common;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IFileService
    {
        Task<string> GetUrlFromS3(string key);

        byte[] ConvertToXlsx(TableView data);
    }
}
