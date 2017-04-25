using System.ServiceModel;
using System.ServiceModel.Web;

namespace Kadena.Services.MailingList
{
    [ServiceContract]
    public interface IMailingListService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UploadFile")]
        ResponseMessage UploadFile(UploadFileData data);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UploadFilePath")]
        ResponseMessage UploadFilePath();
    }
}
