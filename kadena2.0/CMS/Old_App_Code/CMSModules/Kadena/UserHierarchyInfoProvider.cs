using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.SiteProvider;

namespace Kadena
{    
    /// <summary>
    /// Class providing UserHierarchyInfo management.
    /// </summary>
    public partial class UserHierarchyInfoProvider : AbstractInfoProvider<UserHierarchyInfo, UserHierarchyInfoProvider>
    {
        #region "Constructors"

        /// <summary>
        /// Constructor
        /// </summary>
        public UserHierarchyInfoProvider()
            : base(UserHierarchyInfo.TYPEINFO)
        {
        }

        #endregion


        #region "Public methods - Basic"

        /// <summary>
        /// Returns a query for all the UserHierarchyInfo objects.
        /// </summary>
        public static ObjectQuery<UserHierarchyInfo> GetUserHierarchies()
        {
            return ProviderObject.GetUserHierarchiesInternal();
        }


        /// <summary>
        /// Sets (updates or inserts) specified UserHierarchyInfo.
        /// </summary>
        /// <param name="infoObj">UserHierarchyInfo to be set</param>
        public static void SetUserHierarchyInfo(UserHierarchyInfo infoObj)
        {
            ProviderObject.SetUserHierarchyInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes specified UserHierarchyInfo.
        /// </summary>
        /// <param name="infoObj">UserHierarchyInfo to be deleted</param>
        public static void DeleteUserHierarchyInfo(UserHierarchyInfo infoObj)
        {
            ProviderObject.DeleteUserHierarchyInfoInternal(infoObj);
        }

        #endregion


        #region "Public methods - Advanced"

        /// <summary>
        /// Returns a query for all the UserHierarchyInfo objects of a specified site.
        /// </summary>
        /// <param name="siteId">Site ID</param>
        public static ObjectQuery<UserHierarchyInfo> GetUserHierarchies(int siteId)
        {
            return ProviderObject.GetUserHierarchiesInternal(siteId);
        }
        
        #endregion


        #region "Internal methods - Basic"
	
        /// <summary>
        /// Returns a query for all the UserHierarchyInfo objects.
        /// </summary>
        protected virtual ObjectQuery<UserHierarchyInfo> GetUserHierarchiesInternal()
        {
            return GetObjectQuery();
        }    


        /// <summary>
        /// Sets (updates or inserts) specified UserHierarchyInfo.
        /// </summary>
        /// <param name="infoObj">UserHierarchyInfo to be set</param>        
        protected virtual void SetUserHierarchyInfoInternal(UserHierarchyInfo infoObj)
        {
            SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified UserHierarchyInfo.
        /// </summary>
        /// <param name="infoObj">UserHierarchyInfo to be deleted</param>        
        protected virtual void DeleteUserHierarchyInfoInternal(UserHierarchyInfo infoObj)
        {
            DeleteInfo(infoObj);
        }	

        #endregion

        #region "Internal methods - Advanced"


        /// <summary>
        /// Returns a query for all the UserHierarchyInfo objects of a specified site.
        /// </summary>
        /// <param name="siteId">Site ID</param>
        protected virtual ObjectQuery<UserHierarchyInfo> GetUserHierarchiesInternal(int siteId)
        {
            return GetObjectQuery().OnSite(siteId);
        }    
        
        #endregion		
    }
}