using Microsoft.AspNetCore.Mvc;

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
        public async Task<string> Get() => await Models.Flats.Get();

        [HttpGet("ByName/{name}")]
        public async Task<string> GetByName(string name) => await Models.Flats.GetByName(name);

        [HttpGet("BySleep/{count}")]
        public async Task<string> GetBySP(string count) => await Models.Flats.GetBySP(count);
        [HttpGet("ByFull/{val}")]
        public async Task<string> GetByFull(string val) => await Models.Flats.GetByFull(val);

        [HttpGet("Add/{name}/{full}/{sleep}/{cost}")] //пока get-запрос для проверки и тестового заполнения бд, потом перепишу его в post
        public async Task<string> AddKv(string name,string full, string sleep, string cost) => await Models.Flats.AddKv(name, full, sleep, cost);
    }
}
