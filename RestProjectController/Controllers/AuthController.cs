using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;

namespace RestProjectController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        public string Index()
        {
            return "auth xd";
        }
        [HttpGet("all")]
        public async Task<string> Get() => await Models.Account.Get();

        [HttpGet("{id}")]
        public async Task<string> GetByIndex(string id) => await Models.Account.GetByID(id);

        [HttpPost("Register/{name}/{login}:{password}")]
        public async Task<string> Register(string name, string login, string password) => await Models.Account.Create(name, login, password);

        [HttpPatch("Change_Password:{old_pass};{new_pass}"), Authorize]
        public async Task<string> ChangePassword(string old_pass, string new_pass, [FromHeader(Name = "Authorization")] string jwt )
        {
            var login = Models.Account.GetNameJWT(jwt);
            return await Models.Account.ChangePassword(login, old_pass, new_pass);
        }

        [HttpPatch("Delete")]
        public async Task<string> DeleteAcc([FromHeader(Name = "Authorization")] string jwt)
        {
            var login = Models.Account.GetNameJWT(jwt);
            return await Models.Account.DeleteAcc(login);
        }

        [HttpGet("{login}:{password}")]
        public async Task<string> DoLogin(string login, string password) => await Models.Account.DoLogin(login,password);
    }
}