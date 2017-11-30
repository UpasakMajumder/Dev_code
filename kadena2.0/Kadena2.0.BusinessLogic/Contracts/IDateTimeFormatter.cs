using System;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IDateTimeFormatter
    {
        string GetFormatString();
        string GetFormatString(string cultureCode);
        string Format(DateTime dt);
    }
}
