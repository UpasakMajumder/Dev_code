namespace Kadena2.MicroserviceClients.Contracts.Base
{
    public interface IMicroProperties
    {
        string GetServiceUrl(string urlLocationName);

        string GetCustomerName();

        int GetApiVersion(string apiVersionKeyName);
    }
}
