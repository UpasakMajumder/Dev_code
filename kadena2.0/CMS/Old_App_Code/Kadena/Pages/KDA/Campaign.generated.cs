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
using CMS.DocumentEngine.Types.KDA;
using CMS.DocumentEngine;

[assembly: RegisterDocumentType(Campaign.CLASS_NAME, typeof(Campaign))]

namespace CMS.DocumentEngine.Types.KDA
{
	/// <summary>
	/// Represents a content item of type Campaign.
	/// </summary>
	public partial class Campaign : TreeNode
	{
		#region "Constants and variables"

		/// <summary>
		/// The name of the data class.
		/// </summary>
		public const string CLASS_NAME = "KDA.Campaign";


		/// <summary>
		/// The instance of the class that provides extended API for working with Campaign fields.
		/// </summary>
		private readonly CampaignFields mFields;

		#endregion


		#region "Properties"

		/// <summary>
		/// CampaignID.
		/// </summary>
		[DatabaseIDField]
		public int CampaignID
		{
			get
			{
				return ValidationHelper.GetInteger(GetValue("CampaignID"), 0);
			}
			set
			{
				SetValue("CampaignID", value);
			}
		}


		/// <summary>
		/// Fiscal Year.
		/// </summary>
		[DatabaseField]
		public string FiscalYear
		{
			get
			{
				return ValidationHelper.GetString(GetValue("FiscalYear"), "");
			}
			set
			{
				SetValue("FiscalYear", value);
			}
		}


		/// <summary>
		/// IBTF Finalized.
		/// </summary>
		[DatabaseField]
		public bool IBTFFinalized
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("IBTFFinalized"), false);
			}
			set
			{
				SetValue("IBTFFinalized", value);
			}
		}


		/// <summary>
		/// Campaign Name.
		/// </summary>
		[DatabaseField]
		public new string Name
		{
			get
			{
				return ValidationHelper.GetString(GetValue("Name"), "");
			}
			set
			{
				SetValue("Name", value);
			}
		}


		/// <summary>
		/// Campaign Description.
		/// </summary>
		[DatabaseField]
		public string Description
		{
			get
			{
				return ValidationHelper.GetString(GetValue("Description"), "");
			}
			set
			{
				SetValue("Description", value);
			}
		}


		/// <summary>
		/// Start Date.
		/// </summary>
		[DatabaseField]
		public DateTime StartDate
		{
			get
			{
				return ValidationHelper.GetDateTime(GetValue("StartDate"), DateTimeHelper.ZERO_TIME);
			}
			set
			{
				SetValue("StartDate", value);
			}
		}


		/// <summary>
		/// End Date.
		/// </summary>
		[DatabaseField]
		public DateTime EndDate
		{
			get
			{
				return ValidationHelper.GetDateTime(GetValue("EndDate"), DateTimeHelper.ZERO_TIME);
			}
			set
			{
				SetValue("EndDate", value);
			}
		}


		/// <summary>
		/// Status.
		/// </summary>
		[DatabaseField]
		public bool Status
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("Status"), true);
			}
			set
			{
				SetValue("Status", value);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		[DatabaseField]
		public bool CampaignInitiate
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("CampaignInitiate"), false);
			}
			set
			{
				SetValue("CampaignInitiate", value);
			}
		}


		/// <summary>
		/// Open.
		/// </summary>
		[DatabaseField]
		public bool OpenCampaign
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("OpenCampaign"), false);
			}
			set
			{
				SetValue("OpenCampaign", value);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		[DatabaseField]
		public bool GlobalAdminNotified
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("GlobalAdminNotified"), false);
			}
			set
			{
				SetValue("GlobalAdminNotified", value);
			}
		}


		/// <summary>
		/// Close.
		/// </summary>
		[DatabaseField]
		public bool CloseCampaign
		{
			get
			{
				return ValidationHelper.GetBoolean(GetValue("CloseCampaign"), false);
			}
			set
			{
				SetValue("CloseCampaign", value);
			}
		}


		/// <summary>
		/// Gets an object that provides extended API for working with Campaign fields.
		/// </summary>
		[RegisterProperty]
		public CampaignFields Fields
		{
			get
			{
				return mFields;
			}
		}


		/// <summary>
		/// Provides extended API for working with Campaign fields.
		/// </summary>
		[RegisterAllProperties]
		public partial class CampaignFields : AbstractHierarchicalObject<CampaignFields>
		{
			/// <summary>
			/// The content item of type Campaign that is a target of the extended API.
			/// </summary>
			private readonly Campaign mInstance;


			/// <summary>
			/// Initializes a new instance of the <see cref="CampaignFields" /> class with the specified content item of type Campaign.
			/// </summary>
			/// <param name="instance">The content item of type Campaign that is a target of the extended API.</param>
			public CampaignFields(Campaign instance)
			{
				mInstance = instance;
			}


			/// <summary>
			/// CampaignID.
			/// </summary>
			public int ID
			{
				get
				{
					return mInstance.CampaignID;
				}
				set
				{
					mInstance.CampaignID = value;
				}
			}


			/// <summary>
			/// Fiscal Year.
			/// </summary>
			public string FiscalYear
			{
				get
				{
					return mInstance.FiscalYear;
				}
				set
				{
					mInstance.FiscalYear = value;
				}
			}


			/// <summary>
			/// IBTF Finalized.
			/// </summary>
			public bool IBTFFinalized
			{
				get
				{
					return mInstance.IBTFFinalized;
				}
				set
				{
					mInstance.IBTFFinalized = value;
				}
			}


			/// <summary>
			/// Campaign Name.
			/// </summary>
			public string Name
			{
				get
				{
					return mInstance.Name;
				}
				set
				{
					mInstance.Name = value;
				}
			}


			/// <summary>
			/// Campaign Description.
			/// </summary>
			public string Description
			{
				get
				{
					return mInstance.Description;
				}
				set
				{
					mInstance.Description = value;
				}
			}


			/// <summary>
			/// Start Date.
			/// </summary>
			public DateTime StartDate
			{
				get
				{
					return mInstance.StartDate;
				}
				set
				{
					mInstance.StartDate = value;
				}
			}


			/// <summary>
			/// End Date.
			/// </summary>
			public DateTime EndDate
			{
				get
				{
					return mInstance.EndDate;
				}
				set
				{
					mInstance.EndDate = value;
				}
			}


			/// <summary>
			/// Status.
			/// </summary>
			public bool Status
			{
				get
				{
					return mInstance.Status;
				}
				set
				{
					mInstance.Status = value;
				}
			}


			/// <summary>
			/// 
			/// </summary>
			public bool Initiate
			{
				get
				{
					return mInstance.CampaignInitiate;
				}
				set
				{
					mInstance.CampaignInitiate = value;
				}
			}


			/// <summary>
			/// Open.
			/// </summary>
			public bool OpenCampaign
			{
				get
				{
					return mInstance.OpenCampaign;
				}
				set
				{
					mInstance.OpenCampaign = value;
				}
			}


			/// <summary>
			/// 
			/// </summary>
			public bool GlobalAdminNotified
			{
				get
				{
					return mInstance.GlobalAdminNotified;
				}
				set
				{
					mInstance.GlobalAdminNotified = value;
				}
			}


			/// <summary>
			/// Close.
			/// </summary>
			public bool CloseCampaign
			{
				get
				{
					return mInstance.CloseCampaign;
				}
				set
				{
					mInstance.CloseCampaign = value;
				}
			}
		}

		#endregion


		#region "Constructors"

		/// <summary>
		/// Initializes a new instance of the <see cref="Campaign" /> class.
		/// </summary>
		public Campaign() : base(CLASS_NAME)
		{
			mFields = new CampaignFields(this);
		}

		#endregion
	}
}