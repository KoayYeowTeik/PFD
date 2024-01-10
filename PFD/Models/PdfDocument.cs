using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PFD_ASG.Models
{
	public class PdfDocument
	{
		[BsonId]
		public ObjectId Id { get; set; }
		public int UserID { get; set; }
		public string FileName { get; set; }
		public byte[] Content { get; set; }
	}
}
