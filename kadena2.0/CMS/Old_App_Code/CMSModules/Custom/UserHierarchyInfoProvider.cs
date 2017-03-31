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
        /// <param name="userId">User ID</param>
        /// <param name="userId">User ID</param>  
        public static UserHierarchyInfo GetUserHierarchyInfo(int userId, int userId)
        {
            return ProviderObject.GetUserHierarchyInfoInternal(userId, userId);
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
        /// <param name="userId">User ID</param>
        /// <param name="userId">User ID</param>  
        public static void RemoveUserFromUser(int userId, int userId)
        {
            ProviderObject.RemoveUserFromUserInternal(userId, userId);
        }


        /// <summary>
        /// Creates UserHierarchyInfo binding. 
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userId">User ID</param>   
        public static void AddUserToUser(int userId, int userId)
        {
            ProviderObject.AddUserToUserInternal(userId, userId);
        }

        #endregion


        #region "Internal methods"
	
        /// <summary>
        /// Returns the UserHierarchyInfo structure.
        /// Null if binding doesn't exist.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userId">User ID</param>  
        protected virtual UserHierarchyInfo GetUserHierarchyInfoInternal(int userId, int userId)
        {
            return GetSingleObject()
                .WhereEquals("ParentUserId", userId)
                .WhereEquals("ChildUserId", userId);
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
        /// <param name="userId">User ID</param>
        /// <param name="userId">User ID</param>  
        protected virtual void RemoveUserFromUserInternal(int userId, int userId)
        {
            var infoObj = GetUserHierarchyInfo(userId, userId);
			if (infoObj != null) 
			{
				DeleteUserHierarchyInfo(infoObj);
			}
        }


        /// <summary>
        /// Creates UserHierarchyInfo binding. 
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userId">User ID</param>   
        protected virtual void AddUserToUserInternal(int userId, int userId)
        {
            // Create new binding
            var infoObj = new UserHierarchyInfo();
            infoObj.ParentUserId = userId;
			infoObj.ChildUserId = userId;

            // Save to the database
            SetUserHierarchyInfo(infoObj);
        }
       
        #endregion		
    }
}