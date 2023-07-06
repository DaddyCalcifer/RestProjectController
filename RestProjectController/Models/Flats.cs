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
        static MongoClient client = new MongoClient("mongodb://192.168.1.88:1717");
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

        public static Flats FromJSON(string jscode) => JsonConvert.DeserializeObject<Flats>(jscode);

        public static async Task<string> Get()
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument("isDeleted", false)).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> GetByName(string name)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument { { "Name", name }, { "isDeleted", false } }).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> GetByID(string id)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument("_id", ObjectId.Parse(id))).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> GetBySP(string count)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument { { "SleepPlaces", int.Parse(count) }, { "isDeleted", false } }).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> GetByFull(string val)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument { { "fullFlat", Convert.ToBoolean(val) } , { "isDeleted", false } }).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> AddKv(string name, string full, string sleep, string cost)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            bool test; int sl_test; decimal cost_test;

            if (bool.TryParse(full, out test) == false) return "error";
            if (int.TryParse(sleep, out sl_test) == false) return "error";
            if (decimal.TryParse(cost, out cost_test) == false) return "error";

            if (await collection.CountDocumentsAsync(new BsonDocument { { "Name", name }, { "fullFlat", Convert.ToBoolean(full) }, { "SleepPlaces", int.Parse(sleep) }, { "Price", Convert.ToDecimal(cost) } }) == 0)
            {
                await collection.InsertOneAsync(new BsonDocument {
                    { "Name", name },
                    { "fullFlat", Convert.ToBoolean(full) },
                    { "SleepPlaces", int.Parse(sleep) },
                    { "Price", Convert.ToDecimal(cost) },
                    { "CreationDate", DateTime.Now },
                    { "EditedDate", DateTime.Now },

                    { "isDeleted", false }});
            }
            else return "Already exists";
            var Flat = await collection.Find(new BsonDocument { { "Name", name }, { "fullFlat", Convert.ToBoolean(full) }, { "SleepPlaces", int.Parse(sleep) }, { "Price", Convert.ToDecimal(cost) }, { "isDeleted", false } }).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> DeleteKv(string id)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "_id", ObjectId.Parse(id) } }) != 0)
            {
                await collection.UpdateOneAsync(new BsonDocument { { "_id", ObjectId.Parse(id) }}, Builders<BsonDocument>.Update.Set("isDeleted",true));
            }
            var Flat = await collection.Find(new BsonDocument { { "_id", ObjectId.Parse(id) } }).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> PutKv(string id, string name, string full, string sleep, string cost)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "_id", ObjectId.Parse(id) } }) != 0)
            {
                if(name != String.Empty) 
                    await collection.UpdateOneAsync(new BsonDocument { { "_id", ObjectId.Parse(id) } }, Builders<BsonDocument>.Update.Set("Name", name));

                if(full != null) 
                    await collection.UpdateOneAsync(new BsonDocument { { "_id", ObjectId.Parse(id) } }, Builders<BsonDocument>.Update.Set("fullFlat", full));

                if(sleep != null) 
                    await collection.UpdateOneAsync(new BsonDocument { { "_id", ObjectId.Parse(id) } }, Builders<BsonDocument>.Update.Set("SleepPlaces", sleep));

                if(cost != null) 
                    await collection.UpdateOneAsync(new BsonDocument { { "_id", ObjectId.Parse(id) } }, Builders<BsonDocument>.Update.Set("Price", cost));

                await collection.UpdateOneAsync(new BsonDocument { { "_id", ObjectId.Parse(id) } }, Builders<BsonDocument>.Update.Set("EditedDate", DateTime.Now));
            }
            var Flat = await collection.Find(new BsonDocument { { "_id", ObjectId.Parse(id) } }).ToListAsync();
            return Flat.ToJson();
        }
    }
}
