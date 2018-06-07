using CMS.CustomTables;
using CMS.DataEngine;
using CMS.SiteProvider;
using Kadena.Models.ModuleAccess;
using Kadena.Models.Site;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class KenticoModuleMappingProvider : IKenticoModuleMappingProvider
    {
        public IEnumerable<ModuleMapping> Get()
        {
            var modulesTable = "KDA.KadenaModuleAndPageTypeConnection";

            if (DataClassInfoProvider.GetDataClassInfo(modulesTable) == null)
            {
                return Enumerable.Empty<ModuleMapping>();
            }

            var mappingItems = CustomTableItemProvider
                .GetItems(modulesTable)
                .ToArray();

            var mappings = new List<ModuleMapping>(mappingItems.Length);

            foreach (var mappingItem in mappingItems)
            {
                var keyName = mappingItem.GetStringValue("SettingsKeyCodeName", string.Empty);
                if (keyName == string.Empty)
                {
                    continue;
                }

                var itemModuleState = SettingsKeyInfoProvider
                    .GetValue(keyName, SiteContext.CurrentSiteID)
                    .ToLowerInvariant();

                if (Enum.TryParse(itemModuleState, out KadenaModuleState state))
                {
                    mappings.Add(new ModuleMapping
                    {
                        PageType = mappingItem.GetStringValue("PageTypeCodeName", string.Empty),
                        State = state
                    });
                }
            }

            return mappings;
        }
    }
}
