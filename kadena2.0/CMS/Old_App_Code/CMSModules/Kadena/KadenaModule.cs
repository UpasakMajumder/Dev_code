using CMS;
using CMS.DataEngine;
using CMS.MacroEngine;
using Kadena.Old_App_Code.CMSModules.Macros.Kadena;

[assembly: RegisterModule(typeof(Kadena.Old_App_Code.CMSModules.Kadena.KadenaModule))]
namespace Kadena.Old_App_Code.CMSModules.Kadena
{
    public class KadenaModule : Module
    {
        public KadenaModule()
            : base("Kadena") { }

        protected override void OnInit()
        {
            base.OnInit();
            MacroContext.GlobalResolver.SetNamedSourceData("KadenaNamespace", KadenaMacroNamespace.Instance);
        }
    }
}