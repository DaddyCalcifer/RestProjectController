using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestProjectController.Models
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string flatId { get; set; } = null!;
        public DateOnly fromDate { get; set; }
        public int Days = 0!;
        public string ClientID { get; set; } = null!;
        
    }
}
