using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.CreditCard._3DSi.Requests
{
    /// <summary>
    /// Request from 3DSi to Kadena to approve submissionId
    /// </summary>
    public class ApproveRequestDto
    {       
        /// <summary>
        /// A constant value sent from 3DSI
        /// 
        /// AddStoredCreditCard
        /// VerifyAndAddStoredCreditCard
        /// </summary>
        //[Required]
        [RegularExpression("^AddStoredCreditCard$|^VerifyAndAddStoredCreditCard$", ErrorMessage = "Invalid SilentPostType")]
        public string SilentPostType { get; set; }

        /// <summary>
        /// A unique identifier associated with the current attempt to submit data to CardVault.
        /// The client should use this value during the approval stage to look up approval data 
        /// values provided to the user.
        /// </summary>
        //[Required]
        public string SubmissionID { get; set; }

        /// <summary>
        /// The BIN of the credit card submitted to CardVault. This value is only sent to the Approval URL 
        /// if the merchant is configured to have it sent.
        /// </summary>
        public string CardBIN { get; set; }

        /// <summary>
        /// The number of approval values supplied with this submission.
        /// </summary>
        public int APCount { get; set; }

        /// <summary>
        /// An individual approval value. Code at the ApprovalURL should confirm that data provided matches data sent to the user’s browser 
        /// and either approve or deny the request based upon this data.These values are not sent to the ResultURL.
        /// </summary>
        public string AP1 { get; set; }

        /// <summary>
        /// An individual approval value. Code at the ApprovalURL should confirm that data provided matches data sent to the user’s browser 
        /// and either approve or deny the request based upon this data.These values are not sent to the ResultURL.
        /// </summary>
        public string AP2 { get; set; }

        /// <summary>
        /// An individual approval value. Code at the ApprovalURL should confirm that data provided matches data sent to the user’s browser 
        /// and either approve or deny the request based upon this data.These values are not sent to the ResultURL.
        /// </summary>
        public string AP3 { get; set; }

        /// <summary>
        /// An individual approval value. Code at the ApprovalURL should confirm that data provided matches data sent to the user’s browser 
        /// and either approve or deny the request based upon this data.These values are not sent to the ResultURL.
        /// </summary>
        public string AP4 { get; set; }

        /// <summary>
        /// An individual approval value. Code at the ApprovalURL should confirm that data provided matches data sent to the user’s browser 
        /// and either approve or deny the request based upon this data.These values are not sent to the ResultURL.
        /// </summary>
        public string AP5 { get; set; }

        /// <summary>
        /// An individual approval value. Code at the ApprovalURL should confirm that data provided matches data sent to the user’s browser 
        /// and either approve or deny the request based upon this data.These values are not sent to the ResultURL.
        /// </summary>
        public string AP6 { get; set; }

        /// <summary>
        /// An individual approval value. Code at the ApprovalURL should confirm that data provided matches data sent to the user’s browser 
        /// and either approve or deny the request based upon this data.These values are not sent to the ResultURL.
        /// </summary>
        public string AP7 { get; set; }
    }
}
