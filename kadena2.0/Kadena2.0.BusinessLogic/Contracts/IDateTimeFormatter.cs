using System;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IDateTimeFormatter
    {
        string GetFormatString();
        string Format(DateTime dt);
    }
}
