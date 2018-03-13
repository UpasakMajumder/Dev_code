using System;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IIdentityService
    {
        Uri TryAuthenticate(string samlString);
    }
}
