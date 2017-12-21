namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoHelper
    {
        bool ValidateHash(string value, string hash);

        string GetMimeType(string path);
    }
}
