using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Membership;
using Kadena.Models.ModuleAccess;
using Kadena.Models.Site;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.BusinessLogic.Services
{
    public class ModuleAccessService : IModuleAccessService
    {
        private readonly IKenticoSiteProvider kenticoSiteProvider;
        private readonly IKenticoResourceService kenticoResourceService;
        private readonly IKenticoUserProvider kenticoUserProvider;
        private readonly IKenticoRoleProvider kenticoRoleProvider;
        private readonly IKenticoModuleMappingProvider kenticoModuleMappingProvider;

        public ModuleAccessService(
            IKenticoSiteProvider kenticoSiteProvider,
            IKenticoResourceService kenticoResourceService,
            IKenticoUserProvider kenticoUserProvider,
            IKenticoRoleProvider kenticoRoleProvider,
            IKenticoModuleMappingProvider kenticoModuleMappingProvider)
        {
            this.kenticoSiteProvider = kenticoSiteProvider ?? throw new ArgumentNullException(nameof(kenticoSiteProvider));
            this.kenticoResourceService = kenticoResourceService ?? throw new ArgumentNullException(nameof(kenticoResourceService));
            this.kenticoUserProvider = kenticoUserProvider ?? throw new ArgumentNullException(nameof(kenticoUserProvider));
            this.kenticoRoleProvider = kenticoRoleProvider ?? throw new ArgumentNullException(nameof(kenticoRoleProvider));
            this.kenticoModuleMappingProvider = kenticoModuleMappingProvider ?? throw new ArgumentNullException(nameof(kenticoModuleMappingProvider));
        }

        public string GetMainNavigationWhereCondition(KadenaModuleState moduleState)
        {
            var allowedModules = GetAllowedModules(moduleState);

            if (allowedModules.Any())
            {
                var allowedModulesFilter = string.Join(
                    " OR ",
                    allowedModules.Select(m => $"ClassName = N'{m.PageType}'"));
                return allowedModulesFilter;
            }

            var nothingAllowedFilter = "1 = 0";

            return nothingAllowedFilter;
        }

        private List<ModuleMapping> GetAllowedModules(KadenaModuleState moduleState)
        {
            var modules = kenticoModuleMappingProvider.Get()
                            .Where(mm => mm.State == moduleState);
            var allowedModules = modules
                .Where(m => HasUserAccessToModule(m.PageType))
                .ToList();
            return allowedModules;
        }

        private bool HasUserAccessToModule(string moduleName)
        {
            var moduleNameNormalized = moduleName.ToLower();
            switch (moduleNameNormalized)
            {
                case Modules.AdminModule:
                    return IsUserInTWEAdminRole();
                case Modules.OrdersReport:
                    return IsUserInAnyRole(KnownRoles.MarketingManager);
                default:
                    return true;
            }
        }

        private bool IsUserInTWEAdminRole()
        {
            var tweAdminRolesSetting = kenticoResourceService.GetSiteSettingsKey(Settings.KDA_AdminRoles) ?? "";
            var tweAdminRoles = tweAdminRolesSetting.Split(';');
            return IsUserInAnyRole(tweAdminRoles);
        }

        private bool IsUserInAnyRole(params string[] roles)
        {
            if (roles == null || roles.Length == 0)
            {
                return false;
            }

            var user = kenticoUserProvider.GetCurrentUser();
            if (user == null)
            {
                return false;
            }

            var currentSite = kenticoSiteProvider.GetKenticoSite();
            var userRoles = kenticoRoleProvider.GetUserRoles(user.UserId);

            foreach (string role in roles)
            {
                if (userRoles.Any(r => r.CodeName == role && r.SiteID == currentSite.Id))
                {
                    return true;
                }

            }

            return false;
        }

        public bool IsAccessible(string moduleName)
        {
            var allowedModules = GetAllowedModules(KadenaModuleState.enabled);
            var isAccessible = allowedModules.Any(m => m.PageType.ToLowerInvariant() == moduleName);
            return isAccessible;
        }
    }
}
