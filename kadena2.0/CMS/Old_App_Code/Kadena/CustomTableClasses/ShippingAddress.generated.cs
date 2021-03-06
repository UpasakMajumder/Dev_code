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

using System;
using System.Collections.Generic;

using CMS;
using CMS.Base;
using CMS.Helpers;
using CMS.DataEngine;
using CMS.CustomTables.Types.KDA;
using CMS.CustomTables;

[assembly: RegisterCustomTable(ShippingAddressItem.CLASS_NAME, typeof(ShippingAddressItem))]

namespace CMS.CustomTables.Types.KDA
{
    /// <summary>
    /// Represents a content item of type ShippingAddressItem.
    /// </summary>
    public partial class ShippingAddressItem : CustomTableItem
    {
        #region "Constants and variables"

        /// <summary>
        /// The name of the data class.
        /// </summary>
        public const string CLASS_NAME = "KDA.ShippingAddress";


        /// <summary>
        /// The instance of the class that provides extended API for working with ShippingAddressItem fields.
        /// </summary>
        private readonly ShippingAddressItemFields mFields;

        #endregion


        #region "Properties"

        /// <summary>
        /// Address Type ID.
        /// </summary>
        [DatabaseField]
        public int AddressTypeID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("AddressTypeID"), 0);
            }
            set
            {
                SetValue("AddressTypeID", value);
            }
        }


        /// <summary>
        /// Address ID.
        /// </summary>
        [DatabaseField]
        public int COM_AddressID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("COM_AddressID"), 0);
            }
            set
            {
                SetValue("COM_AddressID", value);
            }
        }


        /// <summary>
        /// Business Unit.
        /// </summary>
        [DatabaseField]
        public string BusinessUnit
        {
            get
            {
                return ValidationHelper.GetString(GetValue("BusinessUnit"), "");
            }
            set
            {
                SetValue("BusinessUnit", value);
            }
        }


        /// <summary>
        /// User ID.
        /// </summary>
        [DatabaseField]
        public int UserID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("UserID"), 0);
            }
            set
            {
                SetValue("UserID", value);
            }
        }


        /// <summary>
        /// Email.
        /// </summary>
        [DatabaseField]
        public string Email
        {
            get
            {
                return ValidationHelper.GetString(GetValue("Email"), "");
            }
            set
            {
                SetValue("Email", value);
            }
        }


        /// <summary>
        /// Company Name.
        /// </summary>
        [DatabaseField]
        public string CompanyName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("CompanyName"), "");
            }
            set
            {
                SetValue("CompanyName", value);
            }
        }


        /// <summary>
        /// Gets an object that provides extended API for working with ShippingAddressItem fields.
        /// </summary>
        [RegisterProperty]
        public ShippingAddressItemFields Fields
        {
            get
            {
                return mFields;
            }
        }


        /// <summary>
        /// Provides extended API for working with ShippingAddressItem fields.
        /// </summary>
        [RegisterAllProperties]
        public partial class ShippingAddressItemFields : AbstractHierarchicalObject<ShippingAddressItemFields>
        {
            /// <summary>
            /// The content item of type ShippingAddressItem that is a target of the extended API.
            /// </summary>
            private readonly ShippingAddressItem mInstance;


            /// <summary>
            /// Initializes a new instance of the <see cref="ShippingAddressItemFields" /> class with the specified content item of type ShippingAddressItem.
            /// </summary>
            /// <param name="instance">The content item of type ShippingAddressItem that is a target of the extended API.</param>
            public ShippingAddressItemFields(ShippingAddressItem instance)
            {
                mInstance = instance;
            }


            /// <summary>
            /// Address Type ID.
            /// </summary>
            public int AddressTypeID
            {
                get
                {
                    return mInstance.AddressTypeID;
                }
                set
                {
                    mInstance.AddressTypeID = value;
                }
            }


            /// <summary>
            /// Address ID.
            /// </summary>
            public int COM_AddressID
            {
                get
                {
                    return mInstance.COM_AddressID;
                }
                set
                {
                    mInstance.COM_AddressID = value;
                }
            }


            /// <summary>
            /// Business Unit.
            /// </summary>
            public string BusinessUnit
            {
                get
                {
                    return mInstance.BusinessUnit;
                }
                set
                {
                    mInstance.BusinessUnit = value;
                }
            }


            /// <summary>
            /// User ID.
            /// </summary>
            public int UserID
            {
                get
                {
                    return mInstance.UserID;
                }
                set
                {
                    mInstance.UserID = value;
                }
            }


            /// <summary>
            /// Email.
            /// </summary>
            public string Email
            {
                get
                {
                    return mInstance.Email;
                }
                set
                {
                    mInstance.Email = value;
                }
            }


            /// <summary>
            /// Company Name.
            /// </summary>
            public string CompanyName
            {
                get
                {
                    return mInstance.CompanyName;
                }
                set
                {
                    mInstance.CompanyName = value;
                }
            }
        }

        #endregion


        #region "Constructors"

        /// <summary>
        /// Initializes a new instance of the <see cref="ShippingAddressItem" /> class.
        /// </summary>
        public ShippingAddressItem() : base(CLASS_NAME)
        {
            mFields = new ShippingAddressItemFields(this);
        }

        #endregion
    }
}