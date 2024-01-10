using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PFD_ASG.Models
{
    public class Photo
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int UserID { get; set; }
        public byte[] ImageData { get; set; }
    }
}
