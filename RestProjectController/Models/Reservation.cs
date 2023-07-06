using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace RestProjectController.Models
{
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        static MongoClient client = new MongoClient("mongodb://192.168.1.88:1717");
        public string? Id { get; set; }

        public ObjectId flatId { get; set; } = ObjectId.Empty!;
        public DateTime fromDate { get; set; }
        public int Days = 0!;
        public ObjectId ClientID { get; set; } = ObjectId.Empty!;
        public bool isCancelled { get; set; } = false!;

        public static async Task<string> Get()
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Reservations");

            var reserv = await collection.Find("{}").ToListAsync();
            return reserv.ToJson();
        }
        public static async Task<string> GetByID(string id)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Reservations");

            var reserv = await collection.Find(new BsonDocument("_id", ObjectId.Parse(id))).ToListAsync();
            return reserv.ToJson();
        }
        public static async Task<string> Reserve(ObjectId flat_id, ObjectId owner_id, DateTime Date, string days)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Reservations");

            var users = db.GetCollection<BsonDocument>("Account");
            var flats = db.GetCollection<BsonDocument>("Flats");

            if (await users.CountDocumentsAsync(new BsonDocument { { "_id", owner_id } }) == 0) return "Error: Wrong user id";
            if (await flats.CountDocumentsAsync(new BsonDocument { { "_id", flat_id } }) == 0) return "Error: Wrong flat id";
            if (int.Parse(days) < 1) return "Error: Wrong days value";

            if (await collection.CountDocumentsAsync(new BsonDocument { { "flatId", flat_id }, { "ClientID", owner_id }, { "Date", Date }, { "Days", int.Parse(days) } }) == 0)
            {
                await collection.InsertOneAsync(new BsonDocument { { "flatId", flat_id }, { "ClientID", owner_id }, { "Date", Date }, { "Days", int.Parse(days) }, { "isCancelled", false } });
            }
            else return "Already Exists";
            var Flat = await collection.Find(new BsonDocument { { "flatId", flat_id }, { "ClientID", owner_id }, { "Date", Date }, { "Days", int.Parse(days) } }).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> Cancel(ObjectId id)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Reservations");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "_id", id } }) != 0)
            {
                await collection.UpdateOneAsync(new BsonDocument { { "_id", id } }, Builders<BsonDocument>.Update.Set("isCancelled", true));
            }
            var reserv = await collection.Find(new BsonDocument { { "_id", id } }).ToListAsync();
            return reserv.ToJson();
        }
    }
}
