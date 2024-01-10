using System.ComponentModel.DataAnnotations;

namespace PFD_ASG.Models
{
	public class Users
	{
		[Display (Name = "User ID")]
		public int userID { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string userName { get; set; }

        [Required(ErrorMessage = "LoginID is required")]
        public string loginID { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "ContactNumber is required")]
        public string contactNumber { get; set; }
        public decimal balance { get; set; }
		public string accountNumber { get; set; }
        [Required(ErrorMessage = "DOB is required")]
        [DataType(DataType.Date)]
        public DateTime dob { get; set; }
        public decimal? limitDay { get; set; }
		public decimal? limitWeek { get; set; }
		public decimal? limitMonth { get; set; }
		public string? faceID { get; set; }
		public string? digitalToken { get; set; }
	}
}
