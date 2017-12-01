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

[assembly: RegisterCustomTable(POSCategoryItem.CLASS_NAME, typeof(POSCategoryItem))]

namespace CMS.CustomTables.Types.KDA
{
	/// <summary>
	/// Represents a content item of type POSCategoryItem.
	/// </summary>
	public partial class POSCategoryItem : CustomTableItem
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "KDA.POSCategory";


		/// <summary>
		/// The instance of the class that provides extended API for working with POSCategoryItem fields.
		/// </summary>
		private readonly POSCategoryItemFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// 
		/// </summary>
		[DatabaseField]
		public string PosCategoryName
		{
			get
			{
				return ValidationHelper.GetString(GetValue("PosCategoryName"), "");
			}
			set
			{
				SetValue("PosCategoryName", value);
			}
		}


		


		/// <summary>
		/// Gets an object that provides extended API for working with POSCategoryItem fields.
		/// </summary>
		[RegisterProperty]
		public POSCategoryItemFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with POSCategoryItem fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class POSCategoryItemFields : AbstractHierarchicalObject<POSCategoryItemFields>
		{
			/// <summary>
			/// The content item of type POSCategoryItem that is a target of the extended API.
			/// </summary>
			private readonly POSCategoryItem mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="POSCategoryItemFields" /> class with the specified content item of type POSCategoryItem.
			/// </summary>
			/// <param name="instance">The content item of type POSCategoryItem that is a target of the extended API.</param>
			public POSCategoryItemFields(POSCategoryItem instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// 
			/// </summary>
			public string PosCategoryName
			{
				get
				{
					return mInstance.PosCategoryName;
				}
				set
				{
					mInstance.PosCategoryName = value;
				}
			}


		
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="POSCategoryItem" /> class.
		/// </summary>
		public POSCategoryItem() : base(CLASS_NAME)
		{
			mFields = new POSCategoryItemFields(this);
		}

		#endregion
	}
}