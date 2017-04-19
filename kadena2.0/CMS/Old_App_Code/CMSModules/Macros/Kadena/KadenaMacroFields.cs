using Kadena.Old_App_Code.CMSModules.Macros.Kadena;
using CMS.MacroEngine;

[assembly: CMS.RegisterExtension(typeof(KadenaMacroFields), typeof(KadenaMacroNamespace))]
namespace Kadena.Old_App_Code.CMSModules.Macros.Kadena
{
    public class KadenaMacroFields : MacroFieldContainer
    {
        protected override void RegisterFields()
        {
            base.RegisterFields();

            // Defines a custom macro field in the container
            RegisterField(new MacroField("SetUpPasswordUrl", GetSetUpPasswordUrl));
        }

        public static object GetSetUpPasswordUrl(EvaluationContext context)
        {
            var setUpUrl = CMS.DataEngine.SettingsKeyInfoProvider.GetURLValue("KDA_SetUpPasswordURL", string.Empty);
            return !string.IsNullOrWhiteSpace(setUpUrl) ? setUpUrl : "URL not found";
        }
    }
}