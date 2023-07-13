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

        public static async Task<string> Reserve(Models.Reservation reservation)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Reservations");

            var flats = AppOptions.db.GetCollection<BsonDocument>("Flats");

            if (await flats.CountDocumentsAsync(new BsonDocument { { "_id", reservation.flatId } }) == 0) return "404 - Not found";

            if (await collection.CountDocumentsAsync(new BsonDocument { { "flatId", reservation.flatId }, { "ClientID", reservation.ClientID }, { "fromDate", reservation.fromDate }, { "tillDate", reservation.tillDate } }) == 0)
            {
                await collection.InsertOneAsync(new BsonDocument { { "flatId", reservation.flatId }, { "ClientID", reservation.ClientID }, { "fromDate", reservation.fromDate }, { "tillDate", reservation.tillDate }, { "isCancelled", false } });
            }
            else return "400 - Bad request";
            var Flat = await collection.Find(new BsonDocument { { "flatId", reservation.flatId }, { "ClientID", reservation.ClientID }, { "fromDate", reservation.fromDate }, { "tillDate", reservation.tillDate } }).ToListAsync();
            return Flat.ToJson();
        }

        public static async Task<string> Cancel(DataJWT jwt, ObjectId id)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Reservations");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "_id", id }}) != 0)
            {
                BsonDocument doc = collection.Find(new BsonDocument { { "_id", id } }).FirstOrDefault();

                if ((string)doc["ClientID"] == jwt.Name || jwt.Role == "Admin")
                    await collection.UpdateOneAsync(new BsonDocument { { "_id", id } }, Builders<BsonDocument>.Update.Set("isCancelled", true));
                else return "403 - Forbidden";
            }
            else return "400 - Bad request";
            var reserv = await collection.Find(new BsonDocument { { "_id", id } }).ToListAsync();
            return reserv.ToJson();
        }
    }
}
