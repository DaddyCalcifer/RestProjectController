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

        [JsonProperty("Role")]
        public string Role { get; set; } = String.Empty!;

        [JsonProperty("isDeleted")]
        public bool isDeleted { get; set; } = false;

        [JsonProperty("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("PasswordChangedAt")]
        public DateTime PasswordChangedAt { get; set; }
    }
}
public class DataJWT
{
    public string Name { get; set; }
    public string Role { get; set; }

    public string EXP { get; set; }
    public string ISS { get; set; }
    public string AUD { get; set; }

    public static DataJWT Parse(string jwt)
    {
        var result = new DataJWT();
        var token = new JwtSecurityToken(jwtEncodedString: jwt.Replace("Bearer ", ""));

        result.Name = token.Claims.First(c => c.Type == ClaimTypes.Name).Value;
        result.Role = token.Claims.First(c => c.Type == ClaimTypes.Role).Value;
        result.EXP = token.Claims.First(c => c.Type == "exp").Value;
        result.ISS = token.Claims.First(c => c.Type == "iss").Value;
        result.AUD = token.Claims.First(c => c.Type == "aud").Value;

        return result;
    }
}
