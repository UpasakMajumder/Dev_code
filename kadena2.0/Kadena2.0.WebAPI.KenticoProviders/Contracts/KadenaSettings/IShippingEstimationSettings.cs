namespace Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings
{
    public interface IShippingEstimationSettings
    {
        string SenderAddressLine1 { get; }
        string SenderAddressLine2 { get; }
        string SenderCountry { get; }
        string SenderState { get; }
        string SenderCity { get; }
        string SenderPostal { get; }
    }
}
