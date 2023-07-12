using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace RestProjectController.Models
{
    public class Flats
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("_id")]
        public ObjectId? Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [JsonProperty("Full")]
        public bool fullFlat { get; set; } = false!;

        [JsonProperty("Sleep")]
        public int SleepPlaces { get; set; } = 1!;

        [JsonProperty("Price")]
        public decimal Price { get; set; }

        [JsonProperty("EditedDate")]
        public DateTime? EditedDate { get; set; }

        [JsonProperty("CreationDate")]
        public DateTime? CreationDate { get; set; }

        [JsonProperty("isDeleted")]
        public bool isDeleted { get; set; } = false;
    }
}
