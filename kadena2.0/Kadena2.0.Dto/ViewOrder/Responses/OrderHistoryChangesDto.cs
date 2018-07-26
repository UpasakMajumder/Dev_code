using Kadena.Dto.Common;

namespace Kadena.Dto.ViewOrder.Responses
{
    public class OrderHistoryChangesDto
    {
        public string Title { get; set; }
        public string[] Headers { get; set; }
        public TableCellDto[][] Items { get; set; }
    }
}