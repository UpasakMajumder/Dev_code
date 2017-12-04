using CMS.FormEngine.Web.UI;


namespace Kadena.CMSFormControls.Kadena
{
    public partial class BusinessUnitControl : FormEngineUserControl
    {
        public override object Value
        {
            get
            {
                return hdnbuid.Value;
            }
            set
            {
                hdnbuid.Value = "";
            }
        }
    }
}