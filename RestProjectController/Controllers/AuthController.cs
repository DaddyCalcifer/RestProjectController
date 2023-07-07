using Microsoft.AspNetCore.Mvc;

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

        [HttpPatch("Change_Password:{login};{old_pass};{new_pass}")]
        public async Task<string> ChangePassword(string login, string old_pass, string new_pass) => await Models.Account.ChangePassword(login, old_pass, new_pass);

        [HttpPatch("Delete:{id}")]
        public async Task<string> DeleteAcc(string id) => await Models.Account.DeleteAcc(id);

        [HttpGet("Login/{login}:{password}")]
        public async Task<string> DoLogin(string login, string password) => await Models.Account.DoLogin(login,password);
    }
}