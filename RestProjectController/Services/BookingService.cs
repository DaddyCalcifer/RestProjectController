using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace RestProjectController.Services
{
    public class BookingService
    {
        public static async Task<string> Get()
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Reservations");

            var reserv = await collection.Find("{}").ToListAsync();
            return reserv.ToJson();
        }
        public static async Task<string> GetByID(string id)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Reservations");

            var reserv = await collection.Find(new BsonDocument("_id", ObjectId.Parse(id))).ToListAsync();
            return reserv.ToJson();
        }
        public static async Task<string> Reserve(ObjectId flat_id, string owner_login, DateTime Date, string days)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Reservations");

            var users = AppOptions.db.GetCollection<BsonDocument>("Account");
            var flats = AppOptions.db.GetCollection<BsonDocument>("Flats");

            if (await users.CountDocumentsAsync(new BsonDocument { { "Login", owner_login } }) == 0) return "Error: Wrong user id";
            if (await flats.CountDocumentsAsync(new BsonDocument { { "_id", flat_id } }) == 0) return "Error: Wrong flat id";
            if (int.Parse(days) < 1) return "Error: Wrong days value";

            if (await collection.CountDocumentsAsync(new BsonDocument { { "flatId", flat_id }, { "ClientID", owner_login }, { "Date", Date }, { "Days", int.Parse(days) } }) == 0)
            {
                await collection.InsertOneAsync(new BsonDocument { { "flatId", flat_id }, { "ClientID", owner_login }, { "Date", Date }, { "Days", int.Parse(days) }, { "isCancelled", false } });
            }
            else return "Already Exists";
            var Flat = await collection.Find(new BsonDocument { { "flatId", flat_id }, { "ClientID", owner_login }, { "Date", Date }, { "Days", int.Parse(days) } }).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> Cancel(string user, ObjectId id)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Reservations");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "_id", id }, { "ClientID", user } }) != 0)
            {
                await collection.UpdateOneAsync(new BsonDocument { { "_id", id } }, Builders<BsonDocument>.Update.Set("isCancelled", true));
            }
            else return "Wrong data!";
            var reserv = await collection.Find(new BsonDocument { { "_id", id } }).ToListAsync();
            return reserv.ToJson();
        }
    }
}
