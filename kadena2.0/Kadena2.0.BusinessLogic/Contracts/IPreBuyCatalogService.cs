namespace Kadena.BusinessLogic.Contracts
{
    public interface IPreBuyCatalogService
    {
        byte[] Generate(int campaignId);
    }
}
