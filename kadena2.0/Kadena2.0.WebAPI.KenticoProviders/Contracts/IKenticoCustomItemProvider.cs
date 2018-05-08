namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoCustomItemProvider
    {
        T GetItem<T>(int id, string className);
    }
}
