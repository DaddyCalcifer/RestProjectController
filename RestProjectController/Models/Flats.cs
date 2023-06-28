using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestProjectController.Models
{
    public class Flats
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = null!;
        public bool fullFlat { get; set; } = false!;
        public int SleepPlaces { get; set; } = 1!;
        public decimal Price { get; set; }
    }
}
