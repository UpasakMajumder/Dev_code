namespace Kadena2.MicroserviceClients.Contracts.Base
{
    public interface ISignedRequestClient
    {
        bool SignRequest { get; set; } 

        string AwsGatewayApiRole { get; set; }
    }
}
