using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using CMS.FormEngine.Web.UI;
using CMS.Helpers;
using Kadena.BusinessLogic.Contracts;
using Kadena.Container.Default;
using Kadena.Models.Routing.Request;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Newtonsoft.Json;

namespace Kadena.CMSFormControls.Kadena
{
    public partial class ErpSelector: FormEngineUserControl
    {
        private ErpSelectorValue _value = new ErpSelectorValue();
        private IKenticoErpSystemsProvider _erpSystemsProvider;
        private IKenticoResourceService _resources;
        private IRoutingService _routingService;
        private IKenticoLogger _logger;

        protected void Page_Load(object sender, EventArgs e)
        {
            _erpSystemsProvider = DIContainer.Resolve<IKenticoErpSystemsProvider>();
            _resources = DIContainer.Resolve<IKenticoResourceService>();
            _routingService = DIContainer.Resolve<IRoutingService>();
            _logger = DIContainer.Resolve<IKenticoLogger>();

            labelInput.Text = _resources.GetResourceString("Kadena.FormControls.ERPMapping.customerERPID");
            labelDrp.Text = _resources.GetResourceString("Kadena.FormControls.ERPMapping.SelectERP");

            drpERP.Enabled = Enabled;
            inputCustomerERPID.Enabled = Enabled;

            var data = new List<ListItem>
            {
                new ListItem(_resources.GetResourceString("Kadena.FormControls.ERPMapping.NotSpecified"), "")
            };

            _erpSystemsProvider.GetErpSystems().ForEach(erpSystem =>
            {
                data.Add(new ListItem(erpSystem.DisplayName, erpSystem.CodeName));
            });

            drpERP.SelectedValue = data.Any(x => x.Value == _value.TargetErpCodename) ? _value.TargetErpCodename : null;
            drpERP.DataTextField = "Text";
            drpERP.DataValueField = "Value";
            drpERP.DataSource = data;
            drpERP.DataBind();

            inputCustomerERPID.Text = _value.CustomerErpId;
        }

        public override object Value
        {
            get
            {
                UpdateInternalState();
                Task.Run(async () =>
                {
                    await SyncToMicroservice();
                }).GetAwaiter().GetResult();

                return JsonConvert.SerializeObject(_value);
            }
            set => _value =
                JsonConvert.DeserializeObject<ErpSelectorValue>(ValidationHelper.GetString(value, string.Empty)) ??
                new ErpSelectorValue();
        }

        private async Task SyncToMicroservice()
        {                
            var siteId = Request.QueryString["siteid"];

            // global context
            if (siteId == null)
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
                            SiteId = siteId,
                            Id = _value.MicroserviceId
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
                        SiteId = siteId,
                        Id = _value.MicroserviceId,
                        ErpClientId = _value.CustomerErpId,
                        ErpName = _value.TargetErpCodename
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

        private class ErpSelectorValue
        {
            public string MicroserviceId { get; set; }
            public string TargetErpCodename { get; set; }
            public string CustomerErpId { get; set; }
            public bool ToSync { get; set; }
        }
    }
}