using Kadena.BusinessLogic.Contracts;
using Kadena.Models;
using Kadena.Models.Login;
using Kadena.Models.Membership;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IKenticoUserProvider userProvider;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;

        public UserService(IKenticoUserProvider userProvider, IKenticoResourceService resources, IKenticoDocumentProvider documents)
        {
            this.userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.documents = documents ?? throw new ArgumentNullException(nameof(documents));
        }

        public CheckTaCResult CheckTaC()
        {
            var tacEnabled = resources.GetSettingsKey<bool>(Settings.KDA_TermsAndConditionsLogin);

            var showTaC = false;

            if (tacEnabled)
            {
                var user = userProvider.GetCurrentUser();
                showTaC = IsShowTaC(user);
            }

            return new CheckTaCResult
            {
                Show = showTaC
            };
        }

        private bool IsShowTaC(User user)
        {
            var tacValidFrom = documents.GetTaCValidFrom();
            return user.TermsConditionsAccepted.Date < tacValidFrom.Date;
        }

        public void AcceptTaC() => userProvider.AcceptTaC();
    }
}
