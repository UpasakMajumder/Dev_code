using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.Dto.Logon
{
    public class LogonUserResultDTO
    {
        public bool success { get; set; }
        public string errorMessage { get; set; }
        public string errorPropertyName { get; set; }
    }
}
