namespace Kadena.Dto.ViewOrder.Responses
{
    public class EditOrderDialogDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ValidationMessage { get; set; }
        public string SuccessMessage { get; set; }
        public EditOrderDialogButtonsDTO Buttons { get; set; }
    }
}
