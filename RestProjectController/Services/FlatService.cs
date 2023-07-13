using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace RestProjectController.Services
{
    public class FlatService
    {
        public static async Task<string> Get()
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument("isDeleted", false)).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> GetByName(string name)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument { { "Name", name }, { "isDeleted", false } }).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> GetByID(string id)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument("_id", ObjectId.Parse(id))).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> GetBySP(string count)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument { { "SleepPlaces", int.Parse(count) }, { "isDeleted", false } }).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> GetByFull(string val)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument { { "fullFlat", Convert.ToBoolean(val) }, { "isDeleted", false } }).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> AddKv(Models.Flats flat)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Flats");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "Name", flat.Name }, { "fullFlat", flat.fullFlat }, { "SleepPlaces", flat.SleepPlaces }, { "Price", flat.Price } }) == 0)
            {
                await collection.InsertOneAsync(new BsonDocument {
                    { "Name", flat.Name },
                    { "fullFlat", flat.fullFlat },
                    { "SleepPlaces", flat.SleepPlaces },
                    { "Price", flat.Price },
                    { "CreatedBy", flat.AddedBy },
                    { "CreationDate", flat.CreationDate },
                    { "EditedDate", flat.EditedDate },

                    { "isDeleted", false }});
            }
            else return "Already exists";
            var Flat = await collection.Find(new BsonDocument { { "Name", flat.Name }, { "fullFlat", flat.fullFlat }, { "SleepPlaces", flat.SleepPlaces }, { "Price", flat.Price }, { "isDeleted", false } }).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> DeleteKv(DataJWT jwt, string id)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Flats");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "_id", ObjectId.Parse(id) } }) != 0)
            {
                BsonDocument doc = collection.Find(new BsonDocument { { "_id", ObjectId.Parse(id) } }).FirstOrDefault();

                if (doc["CreatedBy"] == jwt.Name || jwt.Role == "Admin")
                    await collection.UpdateOneAsync(new BsonDocument { { "_id", ObjectId.Parse(id) } }, Builders<BsonDocument>.Update.Set("isDeleted", true));
                else return "403 - Forbidden";
            }
            var Flat = await collection.Find(new BsonDocument { { "_id", ObjectId.Parse(id) } }).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> PutKv(DataJWT jwt, Models.Flats new_flat)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Flats");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "_id", new_flat.Id } }) != 0)
            {
                BsonDocument doc = collection.Find(new BsonDocument { { "_id", new_flat.Id } }).FirstOrDefault();

                if (doc["CreatedBy"] == jwt.Name || jwt.Role == "Admin")
                    await collection.ReplaceOneAsync(new BsonDocument { { "_id", new_flat.Id } }, new BsonDocument {
                    { "Name", new_flat.Name },
                    {"fullFlat", new_flat.fullFlat },
                    {"SleepPlaces", new_flat.SleepPlaces },
                    {"Price", new_flat.Price },
                    { "CreatedBy", doc["CreatedBy"] },
                    { "CreationDate", doc["CreationDate"] },
                    {"EditedDate", DateTime.Now },
                    { "isDeleted", doc["isDeleted"] },});
                else return "403 - Forbidden";
            }
            var Flat = await collection.Find(new BsonDocument { { "_id", new_flat.Id } }).ToListAsync();
            return Flat.ToJson();
        }
    }
}
