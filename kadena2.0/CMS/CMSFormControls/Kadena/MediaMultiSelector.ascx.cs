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
                SetValueToControl(value);
            }
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            var values = ExtractValuesFromControl();
            values.Add("");
            ItemsRepeater.DataSource = values.ToArray();
            ItemsRepeater.DataBind();
        }

        private List<string> ExtractValuesFromControl()
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
                        if (!string.IsNullOrWhiteSpace(ms.Value))
                        {
                            values.Add(ms.Value);
                        }
                    }
                }
            }

            return values;
        }

        private object GetValueFromControl()
        {
            var values = ExtractValuesFromControl();
            var fieldValue = MediaMultiField.CreateFieldValue(values.ToArray());
            return fieldValue;
        }

        private void SetValueToControl(object value)
        {
            if (value == null)
            {
                ItemsRepeater.DataSource = new string[0];
                return;
            }

            ItemsRepeater.DataSource = MediaMultiField.GetValues(value.ToString());
            ItemsRepeater.DataBind();
        }

        private bool ValidateExtensions()
        {
            var extensions = AllowedExtensions.Split(',');
            Predicate<string> hasValidExtension = 
                (file) => extensions.Any(ext => file.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase));

            var files = ExtractValuesFromControl();
            foreach (var file in files)
            {
                if (!hasValidExtension(file))
                {
                    return false;
                }
            }

            return true;
        }
    }
}