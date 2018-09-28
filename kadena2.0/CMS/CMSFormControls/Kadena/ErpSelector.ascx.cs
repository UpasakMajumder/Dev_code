using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using CMS.Ecommerce;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using Kadena.BusinessLogic.Contracts;
using Kadena.Container.Default;
using Kadena.Models;
using Kadena.Models.Routing.Request;
using Kadena.Models.SiteSettings.ErpMapping;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Newtonsoft.Json;

namespace Kadena.CMSFormControls.Kadena
{
    public partial class ErpSelector: FormEngineUserControl
    {
        private ErpSelectorValue _value = new ErpSelectorValue();
        private IErpSystemsService _erpSystemsService;
        private IKenticoResourceService _resources;
        private IRoutingService _routingService;
        private IKenticoLogger _logger;

        protected void Page_Load(object sender, EventArgs e)
        {

            _erpSystemsService = DIContainer.Resolve<IErpSystemsService>();
            _resources = DIContainer.Resolve<IKenticoResourceService>();
            _routingService = DIContainer.Resolve<IRoutingService>();
            _logger = DIContainer.Resolve<IKenticoLogger>();

            if (!IsPostBack)
            {

                labelInput.Text = _resources.GetResourceString("Kadena.FormControls.ERPMapping.customerERPID");
                labelDrp.Text = _resources.GetResourceString("Kadena.FormControls.ERPMapping.SelectERP");

                LoadErpDropDownList();

                inputCustomerERPID.Enabled = Enabled;
                inputCustomerERPID.Text = _value.CustomerErpId;

            }

        }

        private void LoadErpDropDownList()
        {
            var erpListItems = _erpSystemsService.GetAll().Select(x => new ListItem(x.DisplayName, x.CodeName)).ToList();

            erpListItems.Insert(0, new ListItem() { Text = _resources.GetResourceString("Kadena.FormControls.ERPMapping.NotSpecified"), Value = "" });

            drpERP.DataSource = erpListItems;
            drpERP.SelectedValue = erpListItems.Any(x => x.Value == _value.TargetErpCodename) ? _value.TargetErpCodename : null;
            drpERP.DataTextField = "Text";
            drpERP.DataValueField = "Value";
            drpERP.DataBind();
            drpERP.Enabled = Enabled;
        }

        public override object Value
        {
            get
            {
                var paymentOptionInfo = GetPaymentOptionInfo();
                if (!int.TryParse(Request.QueryString["siteid"], out var siteId) && paymentOptionInfo != null)
                {
                    siteId = paymentOptionInfo.PaymentOptionSiteID;
                }

                Task.Run(async () =>
                {
                    await SyncToMicroservice(siteId, paymentOptionInfo);
                }).GetAwaiter().GetResult();

                return JsonConvert.SerializeObject(_value);
            }
            set => _value =
                JsonConvert.DeserializeObject<ErpSelectorValue>(ValidationHelper.GetString(value, string.Empty)) ??
                new ErpSelectorValue();
        }

        private async Task SyncToMicroservice(int siteId, PaymentOptionInfo paymentOptionInfo)
        {

            // global context
            if (siteId == 0)
            {
                _value = new ErpSelectorValue();
                return;
            }

            // nothing to sync
            if (!_value.ToSync)
            {
                return;
            }

            try
            {
                bool toSync;
                // erp mapping not specified
                if (string.IsNullOrWhiteSpace(_value.TargetErpCodename) && string.IsNullOrWhiteSpace(_value.CustomerErpId))
                {
                    // nothing to remove from microservice
                    if (_value.MicroserviceId == null)
                    {
                        toSync = false;
                    }
                    // remove from microservice
                    else
                    {
                        var result = await _routingService.DeleteRouting(new DeleteRouting
                        {
                            SiteId = siteId.ToString(),
                            Id = _value.MicroserviceId,
                        });
                        toSync = result;
                        _value.MicroserviceId = null;
                    }
                }
                else
                {
                    // upsert to microservice
                    var result = await _routingService.SetRouting(new SetRouting
                    {
                        SiteId = siteId.ToString(),
                        Id = _value.MicroserviceId,
                        ErpClientId = _value.CustomerErpId,
                        ErpName = _value.TargetErpCodename,
                        PaymentMethod = GetPaymentOptionName(paymentOptionInfo?.PaymentOptionClassName)
                    });

                    _value.MicroserviceId = result?.Id;
                    toSync = result?.Id == null;
                }

                _value.ToSync = toSync;
            }
            catch (Exception e)
            {
                _logger.LogException(MethodBase.GetCurrentMethod().Name, e);
            }   
        }

        private PaymentOptionInfo GetPaymentOptionInfo()
        {
            if (this.EditedObject is PaymentOptionInfo)
            {
                return (PaymentOptionInfo)this.EditedObject;
            }

            return null;
        }

        private string GetPaymentOptionName(string paymentOptionName)
        {
            if (string.IsNullOrWhiteSpace(paymentOptionName)) { return null; }

            return (new PaymentMethod() { ClassName = paymentOptionName }).ShortClassName;
        }

        private void UpdateInternalState()
        {
            if (!Enabled)
            {
                _value.ToSync = true;
                _value.CustomerErpId = null;
                _value.TargetErpCodename = null;

                return;
            }

            if (_value.TargetErpCodename == drpERP.SelectedItem.Value &&
                _value.CustomerErpId == inputCustomerERPID.Text && 
                _value.ToSync == false)
            {
                return;
            }

            _value.TargetErpCodename = drpERP.SelectedItem.Value;
            _value.CustomerErpId = inputCustomerERPID.Text;
            _value.ToSync = true;
        }

        #region Events
        protected void drpERP_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInternalState();
        }

        #endregion

        protected void inputCustomerERPID_TextChanged(object sender, EventArgs e)
        {
            UpdateInternalState();
        }
    }
}