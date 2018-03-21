using Kadena.Infrastructure.FileConversion;

namespace Kadena.Infrastructure.Contracts
{
    public interface IExcelConvert
    {
        byte[] Convert(Table data);
    }
}
