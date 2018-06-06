using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                var breadcrumbs = new Dictionary<string, string>();
                var doc = DocumentHelper.GetDocument(DocumentId, new TreeProvider(MembershipContext.AuthenticatedUser));

                while (doc != null && doc.Parent != null)
                {
                    breadcrumbs.Add(doc.DocumentName, doc.AbsoluteURL);
                    doc = doc.Parent;
                };

                breadcrumbs.Add(GetString("Kadena.BreadcrumbsHome"), URLHelper.GetFullApplicationUrl());

                ltBreadcrumbs.Text = string.Join("", breadcrumbs.Reverse().Select(kv => string.Format(markupFormat, kv.Value, kv.Key)));
            }
        }
    }
}