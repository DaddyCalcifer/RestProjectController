using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace RestProjectController.Models
{
    public class Flats
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        static MongoClient client = new MongoClient("mongodb://192.168.1.88:1717");
        public string? Id { get; set; }

        public string Name { get; set; } = null!;
        public bool fullFlat { get; set; } = false!;
        public int SleepPlaces { get; set; } = 1!;
        public decimal Price { get; set; }
        public List<DateTime> edits = new List<DateTime>();
        public DateTime? CreationDate { get; set; }

        public static async Task<string> Get()
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find("{}").ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> GetByName(string name)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument("Name", name)).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> GetBySP(string count)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument("SleepPlaces", int.Parse(count))).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> GetByFull(string val)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument("fullFlat", Convert.ToBoolean(val))).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> AddKv(string name, string full, string sleep, string cost)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "Name", name }, { "fullFlat", Convert.ToBoolean(full) }, { "SleepPlaces", int.Parse(sleep) }, { "Price", Convert.ToDecimal(cost) } }) == 0)
            {
                await collection.InsertOneAsync(new BsonDocument { { "Name", name }, { "fullFlat", Convert.ToBoolean(full) }, { "SleepPlaces", int.Parse(sleep) }, { "Price", Convert.ToDecimal(cost) } });
            }
            var users = await collection.Find("{}").ToListAsync();
            var Flat = await collection.Find(new BsonDocument { { "Name", name }, { "fullFlat", Convert.ToBoolean(full) }, { "SleepPlaces", int.Parse(sleep) }, { "Price", Convert.ToDecimal(cost) } }).ToListAsync();
            return Flat.ToJson();
        }
    }
}
