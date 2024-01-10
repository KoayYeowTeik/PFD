using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PFD_ASG.Models
{
	public class TransactionHistory
	{
		[Display (Name = "Record ID")]
		public int recordID { get; set; }
		[Display (Name = "Transaction date & time")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
		[Required (ErrorMessage = "DateTime required for transaction record!")]
		public DateTime transactionTime { get; set; }
		[Display (Name = "Transaction description")]
		[StringLength (255,ErrorMessage = "Description cannot exceed 255 characters!")]
		public string? description { get; set; }
		[Display (Name = "Sender ID")]
		[Required (ErrorMessage = "Sender ID required!")]
		public int senderID { get; set; }
		[Display (Name = "Receiver ID")]
		[Required (ErrorMessage = "Receiver ID required!")]
		public int receiverID { get; set; }
		[Display (Name = "Amount transferred")]
		[Required (ErrorMessage = "Transfer amount required!")]
		[DefaultValue (0)]
		[DisplayFormat (DataFormatString = "{0:###,##0.##}")]
		public decimal amount { get; set; }
		[RegularExpression (@"^(1,2,3)$")]
		public int status { get; set; }
		public string category { get; set; }
    }
}
