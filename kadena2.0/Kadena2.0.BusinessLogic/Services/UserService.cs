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
        private readonly IKenticoSiteProvider siteProvider;

        public UserService(IKenticoUserProvider userProvider, IKenticoResourceService resources, IKenticoDocumentProvider documents
            , IKenticoSiteProvider siteProvider)
        {
            this.userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.documents = documents ?? throw new ArgumentNullException(nameof(documents));
            this.siteProvider = siteProvider ?? throw new ArgumentNullException(nameof(siteProvider));
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

        public void RegisterUser(Registration registration)
        {
            var user = userProvider.GetUser(registration.Email);

            user = new User
            {
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                UserName = registration.Email,
                Email = registration.Email
            };

            var customer = new Customer
            {
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                Email = registration.Email
            };

            userProvider.CreateUser(user, siteProvider.GetKenticoSite().Id);
            customer.Id = userProvider.CreateCustomer(customer);

            userProvider.LinkCustomerToUser(customer.Id, user.UserId);
        }
    }
}
