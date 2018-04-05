using Kadena.Models.TemplatedProduct;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IMailService
    {
        void SendProofMail(EmailProofRequest request);
    }
}