using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.SubmitOrder.MicroserviceRequests
{
    public class CustomerDTO
    {
        
        public int KenticoCustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int KenticoUserID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Customer Number is a mandatory field")]
        public string CustomerNumber { get; set; }
  }
}
