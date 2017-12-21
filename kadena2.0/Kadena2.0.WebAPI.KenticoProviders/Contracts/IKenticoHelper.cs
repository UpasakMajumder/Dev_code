namespace Kadena2.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoHelper
    {
        bool ValidateHash(string value, string hash);
    }
}
