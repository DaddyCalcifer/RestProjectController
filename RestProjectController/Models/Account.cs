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
            
            if (await collection.CountDocumentsAsync(new BsonDocument { { "Name", name }, { "Login", login }, { "Password", password } }) == 0)
            {
                await collection.InsertOneAsync(new BsonDocument { { "Name", name }, { "Login", login }, { "Password", password } });
            }
            var Flat = await collection.Find(new BsonDocument { { "Name", name }, { "Login", login }, { "Password", password } }).ToListAsync();
            return Flat.ToJson();
        }
    }
}
