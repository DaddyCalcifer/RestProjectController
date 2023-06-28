using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;

namespace RestProjectController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlatsController : ControllerBase
    {
        static MongoClient client = new MongoClient("mongodb://localhost:27017");
        public string Index()
        {
            return "xd";
        }
        [HttpGet("all")]
        public async Task<string> Get()
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find("{}").ToListAsync();
            return Flat.ToJson();
        }
        [HttpGet("ByName/{name}")]
        public async Task<string> GetByName(string name)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument("Name", name)).ToListAsync();
            return Flat.ToJson();
        }
        [HttpGet("BySleep/{count}")]
        public async Task<string> GetBySP(string count)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument("SleepPlaces", int.Parse(count))).ToListAsync();
            return Flat.ToJson();
        }
        [HttpGet("ByFull/{val}")]
        public async Task<string> GetByFull(string val)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            var Flat = await collection.Find(new BsonDocument("fullFlat", Convert.ToBoolean(val))).ToListAsync();
            return Flat.ToJson();
        }
        [HttpGet("Add/{name}/{full}/{sleep}/{cost}")] //пока get-запрос для проверки и тестового заполнения бд, потом перепишу его в post
        public async Task<string> AddKv(string name,string full, string sleep, string cost)
        {
            var db = client.GetDatabase("RestApp");
            var collection = db.GetCollection<BsonDocument>("Flats");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "Name", name }, { "fullFlat", Convert.ToBoolean(full) }, { "SleepPlaces", int.Parse(sleep) }, { "Price", Convert.ToDecimal(cost) } }) == 0)
            {
                await collection.InsertOneAsync(new BsonDocument{ { "Name", name},{ "fullFlat",Convert.ToBoolean(full)},{ "SleepPlaces", int.Parse(sleep)}, {"Price", Convert.ToDecimal(cost) } });
            }
            var users = await collection.Find("{}").ToListAsync();
            var Flat = await collection.Find(new BsonDocument { { "Name", name }, { "fullFlat", Convert.ToBoolean(full) }, { "SleepPlaces", int.Parse(sleep) }, { "Price", Convert.ToDecimal(cost) } }).ToListAsync();
            return Flat.ToJson();
        }
    }
}
