using System.ComponentModel.DataAnnotations;

namespace PFD_ASG.Models
{
    public class AccountViewModel
    {
        [Required(ErrorMessage = "UserName is required")]
        public string userName { get; set; }

        [Required(ErrorMessage = "LoginID is required")]
        public string loginID { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "ContactNumber is required")]
        public string contactNumber { get; set; }

        [Required(ErrorMessage = "DOB is required")]
        [DataType(DataType.Date)]
        public DateTime dob { get; set; }

        [Required(ErrorMessage = "cardType is required")]
        public string cardType { get; set; }

        [Required(ErrorMessage = "billingAddress is required")]
        public string billingAddress { get; set; }

        [Required(ErrorMessage = "type is required")]
        public string type { get; set; }
        public string helperID { get; set; }
        public string helperPassword { get; set; }
    }
}
