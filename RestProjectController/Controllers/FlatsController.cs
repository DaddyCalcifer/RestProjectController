using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace RestProjectController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlatsController : ControllerBase
    {
        public string Index()
        {
            return "xd";
        }
        [HttpGet("all")]
        public async Task<string> Get() => await Services.FlatService.Get();

        [HttpGet("ByName:{name}")]
        public async Task<string> GetByName(string name) => await Services.FlatService.GetByName(name);
        [HttpGet("{id}")]
        public async Task<string> GetByIndex(string id) => await Services.FlatService.GetByID(id);

        [HttpGet("BySleep:{count}")]
        public async Task<string> GetBySP(string count) => await Services.FlatService.GetBySP(count);
        [HttpGet("ByFull:{val}")]
        public async Task<string> GetByFull(string val) => await Services.FlatService.GetByFull(val);



        [HttpPost("Add/{name}/{full}/{sleep}/{cost}"), Authorize]
        public async Task<string> AddKv(string name, string full, string sleep, string cost, [FromHeader(Name = "Authorization")] string jwt) => await Services.FlatService.AddKv(name, full, sleep, cost, Services.AuthService.GetNameJWT(jwt));

        [HttpPatch("Delete:{id}"), Authorize]
        public async Task<string> DeleteKv(string id) => await Services.FlatService.DeleteKv(id);

        [HttpPatch("Update:{id};{name};{full};{sleep};{cost}"), Authorize]
        public async Task<string> PutKv(string id, string name, string full, string sleep, string cost) => await Services.FlatService.PutKv(id, name, full, sleep, cost); 
    }
}
