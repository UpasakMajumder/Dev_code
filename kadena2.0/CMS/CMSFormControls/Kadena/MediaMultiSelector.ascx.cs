using CMS.DocumentEngine.Web.UI;
using CMS.FormEngine.Web.UI;
using Kadena.Old_App_Code.Kadena.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Kadena.CMSFormControls.Kadena
{
    public partial class MediaMultiSelector : FormEngineUserControl
    {
        public string AllowedExtensions
        {
            get { return GetValue<string>("AllowedExtensions", null); }
            set { SetValue("AllowedExtensions", value); }
        }

        public override bool IsValid()
        {
            if (!string.IsNullOrWhiteSpace(AllowedExtensions))
            {
                var isValid = ValidateExtensions();
                if (!isValid)
                {
                    ValidationError = "You can only select files of these types - " + AllowedExtensions;
                }
                return isValid;
            }

            return base.IsValid();
        }

        public override object Value
        {
            get
            {
                return GetValueFromControl();
            }

            set
            {
                OldValue = value as string ?? "";
            }
        }

        protected string OldValue;

        protected string CurrentValue
        {
            get
            {
                return ViewState[ClientID] as string ?? "";
            }
            set
            {
                ViewState[ClientID] = value;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitRepeaterItems(OldValue);
            }
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            InitRepeaterItems(CurrentValue);
        }

        private void InitRepeaterItems(string fieldValue)
        {
            var oldValues = MediaMultiField.GetValues(fieldValue);
            ItemsRepeater.DataSource = oldValues;
            ItemsRepeater.DataBind();
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                // hoping posted data has been databound
                var newValues = ExtractValuesFromControl();
                CurrentValue = MediaMultiField.CreateFieldValue(newValues);
            }
            else
            {
                CurrentValue = OldValue;
            }
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            CurrentValue = MediaMultiField.AddValueToField(CurrentValue, "");

            var values = MediaMultiField.GetValues(CurrentValue);
            ItemsRepeater.DataSource = values;
            ItemsRepeater.DataBind();
        }

        private string[] ExtractValuesFromControl(bool skipEmpty = false)
        {
            var values = new List<string>();
            foreach (var item in ItemsRepeater.Items.Cast<RepeaterItem>())
            {
                if (item.ItemType != ListItemType.AlternatingItem && item.ItemType != ListItemType.Item)
                {
                    continue;
                }

                foreach (var ctrl in item.Controls)
                {
                    var ms = ctrl as MediaSelector;
                    if (ms != null)
                    {
                        values.Add(ms.Value);
                    }
                }
            }

            if (skipEmpty)
            {
                values = values.Where(v => !string.IsNullOrWhiteSpace(v)).ToList();
            }

            return values.ToArray();
        }

        private object GetValueFromControl()
        {
            var values = ExtractValuesFromControl(skipEmpty: true).ToArray();
            var fieldValue = MediaMultiField.CreateFieldValue(values);
            return fieldValue;
        }

        private bool ValidateExtensions()
        {
            var files = ExtractValuesFromControl(skipEmpty: true);
            foreach (var file in files)
            {
                if (!MediaMultiField.ValidateExtension(AllowedExtensions, file))
                {
                    return false;
                }
            }

            return true;
        }
    }
}