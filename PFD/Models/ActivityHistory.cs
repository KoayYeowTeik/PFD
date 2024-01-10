using System.ComponentModel.DataAnnotations;

namespace PFD_ASG.Models
{
	public class ActivityHistory
	{
		[Display (Name = "Record ID")]
		public int recordID { get; set; }
		[Display (Name = "Date & Time of action")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
		[Required (ErrorMessage = "Activity date required!")]
		public DateTime activityTime { get; set; }
		[Display (Name = "Action")]
		[StringLength (255,ErrorMessage = "Invalid activity!")]
		[Required (ErrorMessage = "Activty message required!")]
		public string description { get; set; }
		[Display (Name = "User ID")]
		[Required (ErrorMessage ="User ID required for activity!")]
		public int userID { get; set; }
	}
}
