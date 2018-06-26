using Kadena.Models.Common;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IConvert
    {
        byte[] GetBytes(TableView data);
    }
}
