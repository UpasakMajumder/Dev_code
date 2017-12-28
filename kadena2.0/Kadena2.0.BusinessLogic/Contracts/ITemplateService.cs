using Kadena.Models.TemplatedProduct;
using System;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ITemplateService
    {
        Task<bool> UpdateTemplate(Guid templateId, string name, int quantity);
        Task<ProductTemplates> GetTemplatesByProduct(int documentId);
    }
}
