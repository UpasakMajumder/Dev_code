using CMS.CustomTables;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using Kadena.Container.Default;
using Kadena.Models.ModuleAccess;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.CMSWebParts.Kadena.Modules
{
    public partial class ModuleAccessChecker : CMSAbstractWebPart
    {
        #region Public methods

        public override void OnContentLoaded()
        {
            base.OnContentLoaded();
            SetupControl();
        }

        protected void SetupControl()
        {
            if (!StopProcessing && !CurrentUser.CheckPrivilegeLevel(CMS.Base.UserPrivilegeLevelEnum.Editor))
            {                
                var pageTypeModuleMappings = CacheHelper.Cache(cs => GetPageTypeModuleSettingsKeyMappings(), new CacheSettings(20, "Kadena.ModuleAccessChecker._GetPageTypeModuleSettingsKeyMappings" + SiteContext.CurrentSiteName));
                var currentDocument = CurrentDocument;

                while (currentDocument != null && !currentDocument.IsRoot())
                {
                    if (pageTypeModuleMappings.Select(ptmm => ptmm.Item1).Contains(currentDocument.ClassName))
                    {
                        var moduleState = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{pageTypeModuleMappings.Where(ptmm => ptmm.Item1 == currentDocument.ClassName).FirstOrDefault().Item2}");
                        if (moduleState.ToLowerInvariant().Equals(KadenaModuleState.enabled.ToString()))
                        {
                            return;
                        }
                        // module is not enabled - unauthorized accesss
                        var documents = DIContainer.Resolve<IKenticoDocumentProvider>();
                        var url = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.KDA_DisabledModuleUrl");
                        url = documents.GetDocumentUrl(url);
                        Response.Redirect(url);
                    }
                    currentDocument = currentDocument.Parent;
                }
            }
        }

        #endregion

        #region Private methods

        private List<Tuple<string, string>> GetPageTypeModuleSettingsKeyMappings()
        {
            var pageTypeModuleMappings = new List<Tuple<string, string>>();

            var moduleSettingsMappingsDataClassInfo = DataClassInfoProvider.GetDataClassInfo("KDA.KadenaModuleAndPageTypeConnection");
            if (moduleSettingsMappingsDataClassInfo != null)
            {
                var mappingItems = CustomTableItemProvider.GetItems("KDA.KadenaModuleAndPageTypeConnection");

                if (mappingItems != null)
                {
                    foreach (var mappingItem in mappingItems)
                    {
                        pageTypeModuleMappings.Add(new Tuple<string, string>(mappingItem.GetStringValue("PageTypeCodeName", string.Empty), mappingItem.GetStringValue("SettingsKeyCodeName", string.Empty)));
                    }
                }
            }
            return pageTypeModuleMappings;
        }

        #endregion
    }
}