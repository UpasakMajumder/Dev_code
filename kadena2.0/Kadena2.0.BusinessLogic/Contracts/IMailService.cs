using Kadena.Models.TemplatedProduct;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IMailService
    {
        Task SendProofMail(EmailProofRequest request);
    }
}