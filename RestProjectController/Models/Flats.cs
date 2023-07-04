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
        public DateTime? EditedDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool isDeleted { get; set; } = false;
        public Flats() { }
        public Flats(string id)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<Flats>("Flats");

            var obj = collection.Find(new BsonDocument("_id", ObjectId.Parse(id))).ToList();
            this.Name = obj[0].Name;
            this.Id = obj[0].Id;
            this.isDeleted = obj[0].isDeleted;
            this.fullFlat = obj[0].fullFlat;
            this.SleepPlaces = obj[0].SleepPlaces;
            this.Price = obj[0].Price;
            this.CreationDate = obj[0].CreationDate;
            this.EditedDate = obj[0].EditedDate;
        }

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
