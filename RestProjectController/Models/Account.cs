using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace RestProjectController.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        static MongoClient client = new MongoClient("mongodb://192.168.1.88:1717");
        public string? Id { get; set; }
        public string Name { get; set; } = String.Empty!;
        public string Login { get; set; } = String.Empty!;
        public string Password { get; set; } = String.Empty!;
        public bool isDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; }
        public DateTime PasswordChangedAt { get; set; }

        public static async Task<string> Get()
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Account");

            var reserv = await collection.Find("{}").ToListAsync();
            return reserv.ToJson();
        }
        public static async Task<string> GetByID(string id)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Account");

            var reserv = await collection.Find(new BsonDocument("_id", ObjectId.Parse(id))).ToListAsync();
            return reserv.ToJson();
        }
        public static async Task<string> Create(string name, string login, string password)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Account");
            
            if (await collection.CountDocumentsAsync(new BsonDocument { { "Login", login } }) == 0)
            {
                await collection.InsertOneAsync(new BsonDocument { { "Name", name }, { "Login", login }, { "Password", password }, { "CreatedAt", DateTime.Now}, {"isDeleted", false } });
            }
            var Flat = await collection.Find(new BsonDocument { { "Name", name }, { "Login", login }, { "Password", password } }).ToListAsync();
            return Flat.ToJson();
        }
        public static async Task<string> DeleteAcc(string id)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Account");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "_id", ObjectId.Parse(id) } }) != 0)
            {
                await collection.UpdateOneAsync(new BsonDocument { { "_id", ObjectId.Parse(id) } }, Builders<BsonDocument>.Update.Set("isDeleted", true));
            }
            var acc = await collection.Find(new BsonDocument { { "_id", ObjectId.Parse(id) } }).ToListAsync();
            return acc.ToJson();
        }
        public static async Task<string> ChangePassword(string login, string old_pass, string new_pass)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Account");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "Login", login }, {"Password", old_pass } }) != 0)
            {
                await collection.UpdateOneAsync(new BsonDocument { { "Login", login } }, Builders<BsonDocument>.Update.Set("Password", new_pass));
                await collection.UpdateOneAsync(new BsonDocument { { "Login", login } }, Builders<BsonDocument>.Update.Set("PasswordChangedAt", DateTime.Now));
            }
            var acc = await collection.Find(new BsonDocument { { "Login", login } }).ToListAsync();
            return acc.ToJson();
        }
    }
}
