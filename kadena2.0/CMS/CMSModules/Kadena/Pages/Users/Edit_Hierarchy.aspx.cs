using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.UIControls;
using CMS.Helpers;
using CMS.DataEngine;
using System.Data;
using Kadena;
using CMS.Membership;
using CMS.SiteProvider;

namespace CMSApp.CMSModules.Kadena.Pages.Users
{
    public partial class User_Edit_Hierarchy : CMSPage
    {
        private int _currentUserId;

        private string _currentSiteUsers;

        private string _selectedChilds;
        private string _selectedParents;

        protected void Page_Load(object sender, EventArgs e)
        {
            _currentUserId = QueryHelper.GetInteger("objectid", 0);

            var currentSiteUserIds = UserSiteInfoProvider.GetUserSites()
                .WhereEquals("SiteID", SiteContext.CurrentSiteID)
                .And()
                .WhereNotEquals("UserID", _currentUserId);

            _currentSiteUsers = TextHelper.Join(",", DataHelper.GetStringValues(currentSiteUserIds.Tables[0], "UserID"));

            if (_currentUserId > 0)
            {
                SetupControls();

                LoadData();
            }
        }

        private void SetupControls()
        {
            selParent.OnSelectionChanged += SelParent_OnSelectionChanged;
            selChild.OnSelectionChanged += SelChild_OnSelectionChanged;
        }

        private void SelChild_OnSelectionChanged(object sender, EventArgs e)
        {
            bool reloadSecond = false;

            // Remove old items
            string newValues = ValidationHelper.GetString(selChild.Value, null);
            string items = DataHelper.GetNewItemsInList(newValues, _selectedChilds);

            if (!string.IsNullOrEmpty(items))
            {
                var newItems = items.Split(new[] { selChild.ValuesSeparator }, StringSplitOptions.RemoveEmptyEntries);
                // Add all new items to site
                foreach (string item in newItems)
                {
                    int childId = ValidationHelper.GetInteger(item, 0);
                    UserHierarchyInfoProvider.RemoveUserFromUser(_currentUserId, childId);

                    if (_currentUserId == childId)
                    {
                        reloadSecond = true;
                    }
                }
            }

            // Add new items
            items = DataHelper.GetNewItemsInList(_selectedChilds, newValues);
            if (!string.IsNullOrEmpty(items))
            {
                var newItems = items.Split(new[] { selChild.ValuesSeparator }, StringSplitOptions.RemoveEmptyEntries);
                // Add all new items to site
                foreach (string item in newItems)
                {
                    int childId = ValidationHelper.GetInteger(item, 0);
                    UserHierarchyInfoProvider.AddUserToUser(_currentUserId, childId);

                    if (_currentUserId == childId)
                    {
                        reloadSecond = true;
                    }
                }
            }

            // Reload second selector
            if (reloadSecond)
            {
                LoadParentData(true);
                selParent.Reload(true);
            }

            ShowChangesSaved();
        }

        private void SelParent_OnSelectionChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoadData()
        {
            LoadChildData();

            LoadParentData();
        }

        /// <summary>
        /// Loads currently selected child users.
        /// </summary>
        private void LoadChildData(bool forceReload = false)
        {
            DataSet ds = UserHierarchyInfoProvider.GetUserHierarchies().WhereEquals("ParentUserId", _currentUserId);
            if (!DataHelper.DataSourceIsEmpty(ds))
            {
                _selectedChilds = TextHelper.Join(selChild.ValuesSeparator.ToString(), DataHelper.GetStringValues(ds.Tables[0], "ChildUserId"));
                if (!URLHelper.IsPostback() || forceReload)
                {
                    selChild.Value = _selectedChilds;
                }
            }
            else
            {
                if (forceReload)
                {
                    selChild.Value = string.Empty;
                }
            }
        }

        /// <summary>
        /// Loads currently selected parent users.
        /// </summary>
        private void LoadParentData(bool forceReload = false)
        {
            // Get the active child classes
            DataSet ds = UserHierarchyInfoProvider.GetUserHierarchies().WhereEquals("ChildUserId", _currentUserId);
            if (!DataHelper.DataSourceIsEmpty(ds))
            {
                _selectedParents = TextHelper.Join(selParent.ValuesSeparator.ToString(), DataHelper.GetStringValues(ds.Tables[0], "ParentUserId"));
                if (!URLHelper.IsPostback() || forceReload)
                {
                    selParent.Value = _selectedParents;
                }
            }
            else
            {
                if (forceReload)
                {
                    selParent.Value = string.Empty;
                }
            }
        }
    }
}