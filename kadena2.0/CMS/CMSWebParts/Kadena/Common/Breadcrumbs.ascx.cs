using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.CMSWebParts.Kadena.Common
{
    public partial class Breadcrumbs : CMSAbstractWebPart
    {
        private const string markupFormat = "<a href='{0}'>{1}</a>";

        public int DocumentId
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(DocumentId)), 0);
            set => SetValue(nameof(DocumentId), value);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var breadcrumbs = new List<KeyValuePair<string, string>>();
                var doc = DocumentHelper.GetDocument(DocumentId, new TreeProvider(MembershipContext.AuthenticatedUser));

                while (doc?.Parent != null)
                {
                    breadcrumbs.Add(new KeyValuePair<string, string>(doc.DocumentName, doc.AbsoluteURL));
                    doc = doc.Parent;
                };

                breadcrumbs.Add(new KeyValuePair<string, string>(GetString("Kadena.BreadcrumbsHome"), URLHelper.GetFullApplicationUrl()));
                breadcrumbs.Reverse();

                ltBreadcrumbs.Text = string.Join("", breadcrumbs.Select(kv => string.Format(markupFormat, kv.Value, kv.Key)));
            }
        }
    }
}