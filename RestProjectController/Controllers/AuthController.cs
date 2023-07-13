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
        [HttpGet("all"), Authorize]
        public async Task<string> Get([FromHeader(Name = "Authorization")] string jwt)
        {
            var jwt_ = DataJWT.Parse(jwt);
            if (jwt_.Role == "Admin")
                return await Services.AuthService.Get();
            else return "403 - Forbidden";
        }

        [HttpGet("{id}"), Authorize]
        public async Task<string> GetByIndex(string id, [FromHeader(Name = "Authorization")] string jwt)
        {
            var jwt_ = DataJWT.Parse(jwt);
            if (jwt_.Role == "Admin")
               return await Services.AuthService.GetByID(id);
            else return "403 - Forbidden";
        }

        [HttpPost("Register/{name}/{login}:{password}")]
        public async Task<string> Register(string name, string login, string password)
        {
            var account = new Models.Account();
            account.Name = name;
            account.Login = login;
            account.Password = password;
            account.CreatedAt = DateTime.Now;
            account.PasswordChangedAt = DateTime.Now;
            account.Role = "User";
            return await Services.AuthService.Create(account);
        }

        [HttpPatch("Change_Password:{new_pass}"), Authorize]
        public async Task<string> ChangePassword(string new_pass, [FromHeader(Name = "Authorization")] string jwt )
        {
            var jwt_ = DataJWT.Parse(jwt);
            return await Services.AuthService.ChangePassword(jwt_.Name, new_pass);
        }

        [HttpPatch("Delete"), Authorize]
        public async Task<string> DeleteAcc([FromHeader(Name = "Authorization")] string jwt)
        {
            var jwt_ = DataJWT.Parse(jwt);
            return await Services.AuthService.DeleteAcc(jwt_.Name);
        }

        [HttpGet("{login}:{password}")]
        public async Task<string> DoLogin(string login, string password) => await Services.AuthService.DoLogin(login,password);
    }
}