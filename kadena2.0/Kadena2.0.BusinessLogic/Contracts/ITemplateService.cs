using Kadena.Models.TemplatedProduct;
using System;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ITemplateService
    {
        Task<bool> UpdateTemplate(Guid templateId, string name, int quantity);
        Task<ProductTemplates> GetTemplatesByProduct(int documentId);
        Task<Uri> GetPreviewUri(Guid templateId, Guid settingId);
        Task<string> TemplatedProductEditorUrl(int nodeId, int userId, string productType, Guid masterTemplateID, Guid workspaceId, bool use3d);
    }
}
