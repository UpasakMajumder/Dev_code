using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts.SSO
{
    public interface ISaml2Service
    {
        Dictionary<string, IEnumerable<string>> GetAttributes(string samlString);
    }
}
