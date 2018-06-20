namespace Kadena.Models.OrderDetail
{
    public class EditOrderDialog
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ValidationMessage { get; set; }
        public string SuccessMessage { get; set; }
        public EditOrderDialogButtons Buttons { get; set; }
    }
}
