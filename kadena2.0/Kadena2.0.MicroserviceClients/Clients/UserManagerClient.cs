using Kadena.Models.Membership;
using Kadena.Models.SiteSettings;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;

namespace Kadena2.MicroserviceClients.Clients
{
    public class UserManagerClient : SignedClientBase, IUserManagerClient
    {
        private readonly IMicroProperties properties;

        public UserManagerClient(IMicroProperties properties)
        {
            this.properties = properties ?? throw new ArgumentNullException(nameof(properties));
            _serviceVersionSettingKey = Settings.KDA_UserManagerVersion;
        }

        public bool Create(User user)
        {
            throw new NotImplementedException();
        }
    }
}
