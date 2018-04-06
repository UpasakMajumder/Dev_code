namespace Kadena.Models.Orders
{
    public enum OrderStatus
    {
        Initial = 0,
        ArtworkRequested = 3,
        ArtworkReceived = 4,
        ArtworkReceiveError = 5,
        SentToTibco = 10,
        SentToTibcoError = 20,
        Submitted = 30,
        Shipped = 40,
        MoneyCaptured = 53,
        FaiedCaptureMoney = 55,
        Rejected = 60,
        InitialRecordDoesnotExist = 70,
        NA = 80
    }
}
