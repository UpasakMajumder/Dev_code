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

namespace CMSApp.CMSModules.Kadena.Pages.Users
{
    public partial class User_Edit_Hierarchy : CMSPage
    {
        private int _currentUserId;
        private int _currentSiteId;

        private string _currentSiteUsers;

        private string _selectedChilds;
        private string _selectedParents;

        protected void Page_Load(object sender, EventArgs e)
        {
            _currentUserId = QueryHelper.GetInteger("objectid", 0);
            _currentSiteId = CMS.SiteProvider.SiteContext.CurrentSiteID;

            var currentSiteUserIds = UserSiteInfoProvider.GetUserSites()
                .WhereEquals("SiteID", _currentSiteId)
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
                    UserHierarchyInfoProvider.DeleteUserHierarchyInfo(new UserHierarchyInfo
                    {
                        ParentUserId = _currentUserId,
                        ChildUserId = childId,
                        SiteId = _currentSiteId
                    });

                    if (_currentUserId == childId)
                        reloadSecond = true;
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
                    UserHierarchyInfoProvider.SetUserHierarchyInfo(new UserHierarchyInfo
                    {
                        ParentUserId = _currentUserId,
                        ChildUserId = childId,
                        SiteId = _currentSiteId
                    });

                    if (_currentUserId == childId)
                        reloadSecond = true;
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
            bool reloadSecond = false;

            // Remove old items
            string newValues = ValidationHelper.GetString(selParent.Value, null);
            string items = DataHelper.GetNewItemsInList(newValues, _selectedParents);
            if (!string.IsNullOrEmpty(items))
            {
                var newItems = items.Split(new[] { selParent.ValuesSeparator }, StringSplitOptions.RemoveEmptyEntries);
                // Add all new items to site
                foreach (string item in newItems)
                {
                    int parentId = ValidationHelper.GetInteger(item, 0);
                    UserHierarchyInfoProvider.DeleteUserHierarchyInfo(new UserHierarchyInfo
                    {
                        ParentUserId = parentId,
                        ChildUserId = _currentUserId,
                        SiteId = _currentSiteId
                    });

                    if (_currentUserId == parentId)
                        reloadSecond = true;
                }
            }

            // Add new items
            items = DataHelper.GetNewItemsInList(_selectedParents, newValues);
            if (!string.IsNullOrEmpty(items))
            {
                var newItems = items.Split(new[] { selParent.ValuesSeparator }, StringSplitOptions.RemoveEmptyEntries);
                // Add all new items to site
                foreach (string item in newItems)
                {
                    int parentId = ValidationHelper.GetInteger(item, 0);
                    UserHierarchyInfoProvider.SetUserHierarchyInfo(new UserHierarchyInfo
                    {
                        ParentUserId = parentId,
                        ChildUserId = _currentUserId,
                        SiteId = _currentSiteId
                    });

                    if (_currentUserId == parentId)
                        reloadSecond = true;
                }
            }

            // Reload second unigrid
            if (reloadSecond)
            {
                LoadChildData(true);
                selChild.Reload(true);
            }

            ShowChangesSaved();
        }

        private void LoadData()
        {
            LoadChildData();

            LoadParentData();
        }

        /// <summary>
        /// Loads currently selected child users.
        /// </summary>
        /// <param name="forceReload">Forces reload of selected values</param>
        private void LoadChildData(bool forceReload = false)
        {
            // Get the active parent users
            DataSet ds = UserHierarchyInfoProvider.GetUserHierarchies(_currentSiteId).WhereEquals("ParentUserId", _currentUserId);
            if (!DataHelper.DataSourceIsEmpty(ds))
            {
                _selectedChilds = TextHelper.Join(selChild.ValuesSeparator.ToString(), DataHelper.GetStringValues(ds.Tables[0], "ChildUserId"));
                if (!URLHelper.IsPostback() || forceReload)
                    selChild.Value = _selectedChilds;
            }
            else
            {
                if (forceReload)
                    selChild.Value = string.Empty;
            }

            // Filtering data on control for parent users.
            selParent.WhereCondition = $"UserId in ({_currentSiteUsers})"
                + (string.IsNullOrWhiteSpace(_selectedChilds)
                    ? string.Empty
                    : $" and UserId not in ({_selectedChilds})");
        }

        /// <summary>
        /// Loads currently selected parent users.
        /// </summary>
        /// <param name="forceReload">Forces reload of selected values</param>
        private void LoadParentData(bool forceReload = false)
        {
            // Get the active child users
            DataSet ds = UserHierarchyInfoProvider.GetUserHierarchies(_currentSiteId).WhereEquals("ChildUserId", _currentUserId);
            if (!DataHelper.DataSourceIsEmpty(ds))
            {
                _selectedParents = TextHelper.Join(selParent.ValuesSeparator.ToString(), DataHelper.GetStringValues(ds.Tables[0], "ParentUserId"));
                if (!URLHelper.IsPostback() || forceReload)
                    selParent.Value = _selectedParents;
            }
            else
            {
                if (forceReload)
                    selParent.Value = string.Empty;
            }

            // Filtering data on control for child users.
            selChild.WhereCondition = $"UserId in ({_currentSiteUsers})"
                + (string.IsNullOrWhiteSpace(_selectedParents)
                    ? string.Empty
                    : $" and UserId not in ({_selectedParents})");
        }
    }
}