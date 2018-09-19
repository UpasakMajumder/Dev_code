using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.EventLog;
using CMS.UIControls;
using CMS.Helpers;
using System;
using System.Web.UI.WebControls;
using CMS.Core;
using System.Threading;
using System.Security.Principal;
using CMS.DocumentEngine;
using CMS.Localization;
using System.Linq;
using CMS.Ecommerce;

namespace Kadena.CMSModules.Kadena.Pages.Deployment
{
    public partial class MigrationTool : GlobalAdminPage
    {
        private class SerializationOperationState
        {
            public CancellationTokenSource CancellationToken { get; set; }

            public bool Result { get; set; }
        }

        private const string MIGRATION = "Migration";
        private const string MIGRATION_CANCELLED_EVENT_CODE = "MigrationCanceled";
        private const string MIGRATION_FAILED_EVENT_CODE = "MigrationFailed";
        private const string MIGRATION_SUCCESSFUL_EVENT_CODE = "MigrationSuccessful";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            InitAsyncLog();

            var mSerializeAction = new HeaderAction
            {
                Text = "Migrate",
                CommandName = "migrate",
                ButtonStyle = ButtonStyle.Default
            };
            HeaderActions.AddAction(mSerializeAction);
            HeaderActions.ActionPerformed += HeaderActions_ActionPerformed;
        }

        private void HeaderActions_ActionPerformed(object sender, CommandEventArgs e)
        {
            switch (e.CommandName.ToLowerCSafe())
            {
                case "migrate":
                    StartMigration();
                    break;
            }
        }

        private void StartMigration()
        {
            pnlLog.Visible = true;
            ctlAsyncLog.EnsureLog();
            ctlAsyncLog.RunAsync(KDA3437_Migration, WindowsIdentity.GetCurrent());
        }

        private void KDA3437_Migration(object parameters)
        {
            var state = new SerializationOperationState
            {
                CancellationToken = new CancellationTokenSource(),
                Result = false
            };

            ctlAsyncLog.ProcessData.AllowUpdateThroughPersistentMedium = false;
            ctlAsyncLog.ProcessData.Data = state;

            var className = "KDA.Product";
            var products = DocumentHelper.GetDocuments(className)
                            .WhereEquals("ClassName", className)
                            .CheckPermissions()
                            .ToList();

            foreach (var p in products)
            {
                if (state.CancellationToken.IsCancellationRequested)
                {
                    return;
                }
                ctlAsyncLog.AddLog($"Migrating product '{p.DocumentName}' on site '{p.NodeSiteName}'.");
                var sku = SKUInfoProvider.GetSKUInfo(p.NodeSKUID);
                if (p.GetStringValue("ProductType", string.Empty).Contains("KDA.InventoryProduct"))
                {
                    sku.SKUTrackInventory = TrackInventoryTypeEnum.ByProduct;
                }
                else
                {
                    sku.SKUTrackInventory = TrackInventoryTypeEnum.Disabled;
                }
                sku.Update();
            }


            state.Result = true;
        }

        private void InitAsyncLog()
        {
            ctlAsyncLog.OnFinished += OnFinished;
            ctlAsyncLog.OnError += OnError;
            ctlAsyncLog.OnCancel += OnCancel;

            ctlAsyncLog.CancelAction = Cancel;
            ctlAsyncLog.TitleText = "Migrate";
        }

        private void OnFinished(object sender, EventArgs e)
        {
            var state = ctlAsyncLog.ProcessData.Data as SerializationOperationState;
            var result = (state == null) ? false : state.Result;

            if (result)
            {
                ShowConfirmation("Migration successful.", true);
                EventLog(EventType.INFORMATION, MIGRATION_SUCCESSFUL_EVENT_CODE, ctlAsyncLog.Log);
            }
            else
            {
                ShowError("Migration failed.");
                EventLog(EventType.ERROR, MIGRATION_FAILED_EVENT_CODE, "Migration failed.");
            }

            HideDialog();
        }

        private void OnError(object sender, EventArgs e)
        {
            AddError("Migration failed.");
            EventLog(EventType.ERROR, MIGRATION_FAILED_EVENT_CODE, ctlAsyncLog.Log);
            HideDialog();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            ShowError("Migration canceled.");
            EventLog(EventType.ERROR, MIGRATION_CANCELLED_EVENT_CODE, ctlAsyncLog.Log);
            HideDialog();
        }

        private void HideDialog()
        {
            pnlLog.Visible = false;
        }

        private void EventLog(string eventType, string eventCode, string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return;
            }

            EventLogProvider.LogEvent(eventType, MIGRATION, eventCode, description, RequestContext.RawURL, CurrentUser.UserID, CurrentUser.UserName,
                ipAddress: RequestContext.UserHostAddress, machineName: SystemContext.MachineName, urlReferrer: RequestContext.URLReferrer,
                userAgent: RequestContext.UserAgent, eventTime: DateTime.Now, loggingPolicy: LoggingPolicy.DEFAULT);
        }

        private void Cancel()
        {
            var state = ctlAsyncLog.ProcessData.Data as SerializationOperationState;
            if (state == null)
            {
                ShowError("Migration failed.");

                return;
            }

            state.CancellationToken.Cancel();
        }
    }
}