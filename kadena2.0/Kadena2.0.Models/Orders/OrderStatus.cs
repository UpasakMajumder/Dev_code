using System.ComponentModel.DataAnnotations;

namespace Kadena.Models.Orders
{
    public enum OrderStatus
    {
        [Display(Name = "Initial record")]
        Initial = 0,

        [Display(Name = "Waiting for artwork")]
        ArtworkRequested = 3,

        [Display(Name = "Artwork received")]
        ArtworkReceived = 4,

        [Display(Name = "Failed to receive artwork")]
        ArtworkReceiveError = 5,

        [Display(Name = "Sent to Tibco - Waiting for Response")]
        SentToTibco = 10,

        [Display(Name = "Error sending to Tibco")]
        SentToTibcoError = 20,

        [Display(Name = "Submitted")]
        Submitted = 30,

        [Display(Name = "Partially Shipped")]
        PartiallyShipped = 36,

        [Display(Name = "Partially Shipped Error")]
        PartiallyShippedError = 38,

        [Display(Name = "Shipped")]
        Shipped = 40,

        [Display(Name = "Shipped Error")]
        ShippedError = 42,

        [Display(Name = "Money was captured")]
        MoneyCaptured = 53,

        [Display(Name = "Money failed to be captured")]
        FaiedCaptureMoney = 55,

        [Display(Name = "Rejected")]
        Rejected = 60,

        [Display(Name = "Init doesn't exist")]
        InitialRecordDoesnotExist = 70,

        [Display(Name = "Unknown state")]
        NA = 80,

        [Display(Name = "Waiting for approval")]
        WaitingForApproval = 120,

        [Display(Name = "Order approved")]
        Approved = 130,

        [Display(Name = "Order approval rejected")]
        ApprovalRejected = 140,
    }
}
