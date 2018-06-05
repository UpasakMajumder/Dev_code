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
        private readonly IKenticoCustomerProvider customerProvider;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly IKenticoSiteProvider siteProvider;
        private readonly IRoleService roleService;
        private readonly IKenticoMailProvider mailProvider;

        public UserService(IKenticoUserProvider userProvider, IKenticoResourceService resources, IKenticoDocumentProvider documents
            , IKenticoSiteProvider siteProvider, IRoleService roleService, IKenticoMailProvider mailProvider, IKenticoCustomerProvider customerProvider)
        {
            this.userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.documents = documents ?? throw new ArgumentNullException(nameof(documents));
            this.siteProvider = siteProvider ?? throw new ArgumentNullException(nameof(siteProvider));
            this.roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
            this.mailProvider = mailProvider ?? throw new ArgumentNullException(nameof(mailProvider));
            this.customerProvider = customerProvider ?? throw new ArgumentNullException(nameof(customerProvider));
        }

        public CheckTaCResult CheckTaC()
        {
            var tacEnabled = resources.GetSiteSettingsKey<bool>(Settings.KDA_TermsAndConditionsLogin);

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
            if (!resources.GetSiteSettingsKey<bool>(Settings.KDA_EnableRegistration))
            {
                throw new InvalidOperationException("Action denied.");
            }

            var user = new User
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

            var siteId = siteProvider.GetKenticoSite().Id;

            userProvider.CreateUser(user, siteId);
            customer.Id = customerProvider.CreateCustomer(customer);
            userProvider.LinkCustomerToUser(customer.Id, user.UserId);

            userProvider.SetPassword(user.UserId, registration.Password);

            var roles = resources.GetSiteSettingsKey<string>(Settings.KDA_SignupDefaultRole)?.Split(';');
            if (roles != null)
            {
                roleService.AssignRoles(user, siteId, roles);
            }

            mailProvider.SendRegistrationEmails(user);
        }
    }
}
