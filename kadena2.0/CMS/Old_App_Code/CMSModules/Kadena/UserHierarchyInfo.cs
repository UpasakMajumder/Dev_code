using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using Kadena;

[assembly: RegisterObjectType(typeof(UserHierarchyInfo), UserHierarchyInfo.OBJECT_TYPE)]

namespace Kadena
{
    /// <summary>
    /// UserHierarchyInfo data container class.
    /// </summary>
    [Serializable]
    public partial class UserHierarchyInfo : AbstractInfo<UserHierarchyInfo>
    {
        #region "Type information"
		
        /// <summary>
        /// Object type
        /// </summary>
        public const string OBJECT_TYPE = "kadena.userhierarchy";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(UserHierarchyInfoProvider), OBJECT_TYPE, "Kadena.UserHierarchy", null, null, null, null, null, null, null, "ParentUserId", "cms.user")
        {
			ModuleName = "Kadena",
			TouchCacheDependencies = true,
            DependsOn = new List<ObjectDependency>() 
			{
			    new ObjectDependency("ChildUserId", "cms.user", ObjectDependencyEnum.Binding), 
            },
            IsBinding = true
        };

        #endregion


        #region "Properties"

        /// <summary>
        /// Parent user id
        /// </summary>
        [DatabaseField]
        public virtual int ParentUserId
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ParentUserId"), 0);
            }
            set
            {
                SetValue("ParentUserId", value);
            }
        }


        /// <summary>
        /// Child user id
        /// </summary>
        [DatabaseField]
        public virtual int ChildUserId
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ChildUserId"), 0);
            }
            set
            {
                SetValue("ChildUserId", value);
            }
        }

        #endregion


        #region "Type based properties and methods"

        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            UserHierarchyInfoProvider.DeleteUserHierarchyInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            UserHierarchyInfoProvider.SetUserHierarchyInfo(this);
        }

        #endregion


        #region "Constructors"
		
		/// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected UserHierarchyInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates an empty UserHierarchyInfo object.
        /// </summary>
        public UserHierarchyInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates a new UserHierarchyInfo object from the given DataRow.
        /// </summary>
        /// <param name="dr">DataRow with the object data</param>
        public UserHierarchyInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }

        #endregion
    }
}