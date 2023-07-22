using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestProjectController
{
    public class AppOptions
    {
        public const string ISSUER = "nautilus"; // издатель токена
        public const string AUDIENCE = "bookingService"; // потребитель токена
        const string KEY = "y5bFJUFQjWvk+4vee2CxYHBJ6M3hKmwh7+Qw8g665F3o/sOC24tMa/5BMqhmUkav";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(KEY));


        public static readonly MongoClient client = new MongoClient("mongodb://192.168.1.88:27017");
        public static readonly IMongoDatabase db = client.GetDatabase("RestApp");
    }
}
