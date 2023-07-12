using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestProjectController.Models
{
    [JsonObject]
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        [JsonProperty("_id")]
        public ObjectId? Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; } = String.Empty!;

        [JsonProperty("Login")]
        public string Login { get; set; } = String.Empty!;

        [JsonProperty("Password")]
        public string Password { get; set; } = String.Empty!;

        [JsonProperty("isDeleted")]
        public bool isDeleted { get; set; } = false;

        [JsonProperty("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("PasswordChangedAt")]
        public DateTime PasswordChangedAt { get; set; }
    }
}
