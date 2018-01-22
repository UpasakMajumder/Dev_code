//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at http://docs.kentico.com.
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using CMS;
using CMS.Base;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.CustomTables;
using Kadena.CustomTables;

[assembly: RegisterCustomTable(EnvironmentItem.CLASS_NAME, typeof(EnvironmentItem))]

namespace Kadena.CustomTables
{
    /// <summary>
    /// Represents a content item of type EnvironmentItem.
    /// </summary>
    public partial class EnvironmentItem : CustomTableItem
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "KDA.Environment";


        /// <summary>
        /// The instance of the class that provides extended API for working with EnvironmentItem fields.
        /// </summary>
        private readonly EnvironmentItemFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// Code name.
        /// </summary>
        [DatabaseField]
        public string CodeName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("CodeName"), "");
            }
            set
            {
                SetValue("CodeName", value);
            }
        }


        /// <summary>
        /// Display name.
        /// </summary>
        [DatabaseField]
        public string DisplayName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("DisplayName"), "");
            }
            set
            {
                SetValue("DisplayName", value);
            }
        }


        /// <summary>
        /// Amazon S3 folder.
        /// </summary>
        [DatabaseField]
        public string AmazonS3Folder
        {
            get
            {
                return ValidationHelper.GetString(GetValue("AmazonS3Folder"), "");
            }
            set
            {
                SetValue("AmazonS3Folder", value);
            }
        }


        /// <summary>
        /// List of folders to be excluded from storing in Amazon S3 divided by semicolon.
        /// </summary>
        [DatabaseField]
        public string AmazonS3ExcludedPaths
        {
            get
            {
                return ValidationHelper.GetString(GetValue("AmazonS3ExcludedPaths"), "");
            }
            set
            {
                SetValue("AmazonS3ExcludedPaths", value);
            }
        }


        /// <summary>
        /// Gets an object that provides extended API for working with EnvironmentItem fields.
        /// </summary>
        [RegisterProperty]
        public EnvironmentItemFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with EnvironmentItem fields.
        /// </summary>
        [RegisterAllProperties]
        public partial class EnvironmentItemFields : AbstractHierarchicalObject<EnvironmentItemFields>
        {
            /// <summary>
            /// The content item of type EnvironmentItem that is a target of the extended API.
            /// </summary>
            private readonly EnvironmentItem mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="EnvironmentItemFields" /> class with the specified content item of type EnvironmentItem.
            /// </summary>
            /// <param name="instance">The content item of type EnvironmentItem that is a target of the extended API.</param>
            public EnvironmentItemFields(EnvironmentItem instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// Code name.
            /// </summary>
            public string CodeName
            {
                get
                {
                    return mInstance.CodeName;
                }
                set
                {
                    mInstance.CodeName = value;
                }
            }


            /// <summary>
            /// Display name.
            /// </summary>
            public string DisplayName
            {
                get
                {
                    return mInstance.DisplayName;
                }
                set
                {
                    mInstance.DisplayName = value;
                }
            }


            /// <summary>
            /// Amazon S3 folder.
            /// </summary>
            public string AmazonS3Folder
            {
                get
                {
                    return mInstance.AmazonS3Folder;
                }
                set
                {
                    mInstance.AmazonS3Folder = value;
                }
            }


            /// <summary>
            /// List of folders to be excluded from storing in Amazon S3 divided by semicolon.
            /// </summary>
            public string AmazonS3ExcludedPaths
            {
                get
                {
                    return mInstance.AmazonS3ExcludedPaths;
                }
                set
                {
                    mInstance.AmazonS3ExcludedPaths = value;
                }
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentItem" /> class.
        /// </summary>
        public EnvironmentItem() : base(CLASS_NAME)
        {
            mFields = new EnvironmentItemFields(this);
        }

        #endregion
    }
}