using System;
using System.Linq;

using CMS.DataEngine;

namespace Custom
{    
    /// <summary>
    /// Class providing UserHierarchyInfo management.
    /// </summary>
    public partial class UserHierarchyInfoProvider : AbstractInfoProvider<UserHierarchyInfo, UserHierarchyInfoProvider>
    {
        #region "Public methods"

		/// <summary>
        /// Returns all UserHierarchyInfo bindings.
        /// </summary>
        public static ObjectQuery<UserHierarchyInfo> GetUserHierarchies()
        {
            return ProviderObject.GetObjectQuery();
        }


        /// <summary>
        /// Returns UserHierarchyInfo binding structure.
        /// </summary>
        /// <param name="parentUserId">User ID</param>
        /// <param name="childUserId">User ID</param>  
        public static UserHierarchyInfo GetUserHierarchyInfo(int parentUserId, int childUserId)
        {
            return ProviderObject.GetUserHierarchyInfoInternal(parentUserId, childUserId);
        }


        /// <summary>
        /// Sets specified UserHierarchyInfo.
        /// </summary>
        /// <param name="infoObj">UserHierarchyInfo to set</param>
        public static void SetUserHierarchyInfo(UserHierarchyInfo infoObj)
        {
            ProviderObject.SetUserHierarchyInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes specified UserHierarchyInfo binding.
        /// </summary>
        /// <param name="infoObj">UserHierarchyInfo object</param>
        public static void DeleteUserHierarchyInfo(UserHierarchyInfo infoObj)
        {
            ProviderObject.DeleteUserHierarchyInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes UserHierarchyInfo binding.
        /// </summary>
        /// <param name="parentUserId">User ID</param>
        /// <param name="childUserId">User ID</param>  
        public static void RemoveUserFromUser(int parentUserId, int childUserId)
        {
            ProviderObject.RemoveUserFromUserInternal(parentUserId, childUserId);
        }


        /// <summary>
        /// Creates UserHierarchyInfo binding. 
        /// </summary>
        /// <param name="parentUserId">User ID</param>
        /// <param name="childUserId">User ID</param>   
        public static void AddUserToUser(int parentUserId, int childUserId)
        {
            ProviderObject.AddUserToUserInternal(parentUserId, childUserId);
        }

        #endregion


        #region "Internal methods"

        /// <summary>
        /// Returns the UserHierarchyInfo structure.
        /// Null if binding doesn't exist.
        /// </summary>
        /// <param name="parentUserId">User ID</param>
        /// <param name="childUserId">User ID</param>  
        protected virtual UserHierarchyInfo GetUserHierarchyInfoInternal(int parentUserId, int childUserId)
        {
            return GetSingleObject()
                .WhereEquals("ParentUserId", parentUserId)
                .WhereEquals("ChildUserId", childUserId);
        }


        /// <summary>
        /// Sets specified UserHierarchyInfo binding.
        /// </summary>
        /// <param name="infoObj">UserHierarchyInfo object</param>
        protected virtual void SetUserHierarchyInfoInternal(UserHierarchyInfo infoObj)
        {
            SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified UserHierarchyInfo.
        /// </summary>
        /// <param name="infoObj">UserHierarchyInfo object</param>
        protected virtual void DeleteUserHierarchyInfoInternal(UserHierarchyInfo infoObj)
        {
            DeleteInfo(infoObj);
        }


        /// <summary>
        /// Deletes UserHierarchyInfo binding.
        /// </summary>
        /// <param name="parentUserId">User ID</param>
        /// <param name="childUserId">User ID</param>  
        protected virtual void RemoveUserFromUserInternal(int parentUserId, int childUserId)
        {
            var infoObj = GetUserHierarchyInfo(parentUserId, childUserId);
			if (infoObj != null) 
			{
				DeleteUserHierarchyInfo(infoObj);
			}
        }


        /// <summary>
        /// Creates UserHierarchyInfo binding. 
        /// </summary>
        /// <param name="parentUserId">User ID</param>
        /// <param name="childUserId">User ID</param>   
        protected virtual void AddUserToUserInternal(int parentUserId, int childUserId)
        {
            // Create new binding
            var infoObj = new UserHierarchyInfo();
            infoObj.ParentUserId = parentUserId;
			infoObj.ChildUserId = childUserId;

            // Save to the database
            SetUserHierarchyInfo(infoObj);
        }
       
        #endregion		
    }
}