using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestProjectController.Services
{
    public class AuthService
    {
        public static async Task<string> Get()
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Account");

            var reserv = await collection.Find("{}").ToListAsync();
            return reserv.ToJson();
        }
        public static async Task<string> GetByID(string id)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Account");

            var reserv = await collection.Find(new BsonDocument("_id", ObjectId.Parse(id))).ToListAsync();
            return reserv.ToJson();
        }
        public static async Task<string> Create(string name, string login, string password)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Account");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "Login", login }, { "isDeleted", true } }) == 1)
            {
                await collection.UpdateOneAsync(new BsonDocument { { "Login", login } }, Builders<BsonDocument>.Update.Set("Name", name));
                await collection.UpdateOneAsync(new BsonDocument { { "Login", login } }, Builders<BsonDocument>.Update.Set("Password", password));
                await collection.UpdateOneAsync(new BsonDocument { { "Login", login } }, Builders<BsonDocument>.Update.Set("CreatedAt", DateTime.Now));
                await collection.UpdateOneAsync(new BsonDocument { { "Login", login } }, Builders<BsonDocument>.Update.Set("isDeleted", false));
            }
            if (await collection.CountDocumentsAsync(new BsonDocument { { "Login", login } }) == 0)
            {
                await collection.InsertOneAsync(new BsonDocument { { "Name", name }, { "Login", login }, { "Password", password }, { "CreatedAt", DateTime.Now }, { "isDeleted", false } });
            }
            var Flat = await collection.Find(new BsonDocument { { "Login", login } }).ToListAsync();
            return Flat.ToJson();
        }
        public static string CreateJWT(string username)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AppOptions.ISSUER,
                    audience: AppOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AppOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        public static async Task<string> DoLogin(string login, string password)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Account");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "Login", login }, { "Password", password }, { "isDeleted", false } }) != 0)
                return CreateJWT(login);
            else return "Wrong account data!";
        }
        public static async Task<string> DeleteAcc(string login)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Account");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "Login", login } }) != 0)
            {
                await collection.UpdateOneAsync(new BsonDocument { { "Login", login } }, Builders<BsonDocument>.Update.Set("isDeleted", true));
            }
            var acc = await collection.Find(new BsonDocument { { "Login", login } }).ToListAsync();
            return acc.ToJson();
        }
        public static async Task<string> ChangePassword(string login, string old_pass, string new_pass)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Account");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "Login", login }, { "Password", old_pass } }) != 0)
            {
                await collection.UpdateOneAsync(new BsonDocument { { "Login", login } }, Builders<BsonDocument>.Update.Set("Password", new_pass));
                await collection.UpdateOneAsync(new BsonDocument { { "Login", login } }, Builders<BsonDocument>.Update.Set("PasswordChangedAt", DateTime.Now));
            }
            var acc = await collection.Find(new BsonDocument { { "Login", login } }).ToListAsync();
            return acc.ToJson();
        }
        public static string GetNameJWT(string jwt)
        {
            var token = new JwtSecurityToken(jwtEncodedString: jwt.Replace("Bearer ", ""));
            string name = token.Claims.First(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
            return name;
        }
    }
}
