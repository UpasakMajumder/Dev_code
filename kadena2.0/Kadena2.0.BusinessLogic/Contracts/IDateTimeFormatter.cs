using System;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IDateTimeFormatter
    {
        string Format(DateTime dt);
    }
}
