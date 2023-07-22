using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;


namespace RestProjectController.Models
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public ObjectId flatId { get; set; } = ObjectId.Empty!;
        public DateTime fromDate { get; set; }
        public DateTime tillDate { get; set; }
        public string ClientID { get; set; } = String.Empty!;
        public bool isCancelled { get; set; } = false!;
    }
}
