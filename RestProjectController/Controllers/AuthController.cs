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

        [HttpPost("Register/{name}/{login}:{password}")] //пока get-запрос для проверки и тестового заполнения бд, потом перепишу его в post
        public async Task<string> Register(string name, string login, string password) => await Models.Account.Create(name, login, password);
    }
}