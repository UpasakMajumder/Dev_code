using Kadena.Dto.Common;

namespace Kadena.Dto.ViewOrder.Responses
{
    public class OrderActionsDTO
    {
        public DialogButtonDTO<DialogDTO> Accept { get; set; }
        public DialogButtonDTO<DialogDTO> Reject { get; set; }
        public TitleValuePairDto<string> Comment { get; set; }
    }
}
