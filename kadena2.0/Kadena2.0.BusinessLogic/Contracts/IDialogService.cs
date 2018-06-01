using Kadena.Models.Settings;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IDialogService
    {
        IEnumerable<DialogField> GetAddressFields();
    }
}
