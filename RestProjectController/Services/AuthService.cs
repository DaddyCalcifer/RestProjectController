using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCrypt.Net;

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
        public static async Task<string> Create(Models.Account acc)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Account");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "Login", acc.Login }, { "isDeleted", true } }) == 1)
            {
                await collection.ReplaceOneAsync(new BsonDocument { { "Login", acc.Login } }, new BsonDocument { 
                    { "Name", acc.Name }, 
                    { "Login", acc.Login }, 
                    { "Password", BCrypt.Net.BCrypt.HashPassword(acc.Password) }, 
                    { "CreatedAt", acc.CreatedAt },
                    { "Role", acc.Role},
                    { "isDeleted", false }, 
                    {"PasswordChangedAt", acc.PasswordChangedAt } });
            }
            if (await collection.CountDocumentsAsync(new BsonDocument { { "Login", acc.Login } }) == 0)
            {
                await collection.InsertOneAsync(new BsonDocument { 
                    { "Name", acc.Name }, 
                    { "Login", acc.Login }, 
                    { "Password", BCrypt.Net.BCrypt.HashPassword(acc.Password) }, 
                    { "CreatedAt", acc.CreatedAt },
                    { "Role", acc.Role},
                    { "isDeleted", false }, 
                    {"PasswordChangedAt", acc.PasswordChangedAt } });
            }
            var Flat = await collection.Find(new BsonDocument { { "Login", acc.Login } }).ToListAsync();
            return Flat.ToJson();
        }
        public static string CreateJWT(string username,string role)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Role, role) };
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

            BsonDocument doc = collection.Find(new BsonDocument { { "Login", login }, { "isDeleted", false } }).FirstOrDefault();
            if(BCrypt.Net.BCrypt.Verify(password, (string)doc["Password"]))
                return CreateJWT(login, (string)doc["Role"]);
            else return "404 - Not found";
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
        public static async Task<string> ChangePassword(string login, string new_pass)
        {
            var collection = AppOptions.db.GetCollection<BsonDocument>("Account");

            if (await collection.CountDocumentsAsync(new BsonDocument { { "Login", login } }) != 0)
            {
                await collection.UpdateOneAsync(new BsonDocument { { "Login", login } }, Builders<BsonDocument>.Update.Set("Password", new_pass));
                await collection.UpdateOneAsync(new BsonDocument { { "Login", login } }, Builders<BsonDocument>.Update.Set("PasswordChangedAt", DateTime.Now));
            }
            var acc = await collection.Find(new BsonDocument { { "Login", login } }).ToListAsync();
            return acc.ToJson();
        }
    }
}
