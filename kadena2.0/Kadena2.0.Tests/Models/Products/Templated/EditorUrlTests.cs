using Kadena.Models.TemplatedProduct;
using Xunit;

namespace Kadena.Tests.Models.Products.Templated
{
    public class EditorUrlTests
    {
        [Theory]
        [InlineData("/editor?documentId=10&nodeId=20&templateId=30&workspaceid=40&use3d=True&quantity=2&containerId=60&customName=asdf",
            "/editor", 10, 20, "30", "40", true, 2, "60", "asdf")]
        [InlineData("/editor?documentId=10&nodeId=20&templateId=30&workspaceid=40&use3d=True&containerId=60&customName=asdf",
            "/editor", 10, 20, "30", "40", true, 0, "60", "asdf")]
        [InlineData("/editor?documentId=10&nodeId=20&templateId=30&workspaceid=40&use3d=True&quantity=2&customName=asdf",
            "/editor", 10, 20, "30", "40", true, 2, null, "asdf")]
        [InlineData("/editor?documentId=10&nodeId=20&templateId=30&workspaceid=40&use3d=True&quantity=2&containerId=60",
            "/editor", 10, 20, "30", "40", true, 2, "60", "")]
        public void Create_ShouldAddEditorParameters(string expectedUrl,
            string productEditorBaseUrl, int documentId, int nodeId, string templateId, string workspaceid,
            bool use3d, int quantity, string containerId, string customName)
        {
            var result = EditorUrl.Create(productEditorBaseUrl,
                documentId, nodeId, templateId, workspaceid,
                quantity, use3d, containerId, customName);
            Assert.Equal(expectedUrl, result);
        }
    }
}
