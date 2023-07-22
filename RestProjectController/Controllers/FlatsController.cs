using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

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



        [HttpPost("Add:{name};{full};{sleep};{cost}"), Authorize]
        public async Task<string> AddKv(string name, string full, string sleep, string cost, [FromHeader(Name = "Authorization")] string jwt)
        {
            bool check_full; int check_sleep; decimal check_price;
            if (bool.TryParse(full, out check_full) && int.TryParse(sleep, out check_sleep) && decimal.TryParse(cost, out check_price))
            {
                var flat = new Models.Flats();
                flat.Name = name;
                flat.fullFlat = check_full;
                flat.SleepPlaces = check_sleep;
                flat.Price = check_price;
                flat.AddedBy = DataJWT.Parse(jwt).Name;
                flat.CreationDate = DateTime.Now;
                flat.EditedDate = DateTime.Now;
                return await Services.FlatService.AddKv(flat);
            }
            else return "400 - Bad request";
        }

        [HttpPatch("Delete:{id}"), Authorize]
        public async Task<string> DeleteKv(string id, [FromHeader(Name = "Authorization")] string jwt)
        {
            DataJWT jwtdata = DataJWT.Parse(jwt);
            return await Services.FlatService.DeleteKv(jwtdata, id);
        }

        [HttpPut("Update:{id};{name};{full};{sleep};{cost}"), Authorize]
        public async Task<string> PutKv(string id, string name, string full, string sleep, string cost, [FromHeader(Name = "Authorization")] string jwt)
        {
            bool check_full; int check_sleep; decimal check_price;
            var jwtdata = DataJWT.Parse(jwt);
            if (bool.TryParse(full, out check_full) && int.TryParse(sleep, out check_sleep) && decimal.TryParse(cost, out check_price))
            {
                var flat = new Models.Flats();
                flat.Id = ObjectId.Parse(id);
                flat.Name = name;
                flat.fullFlat = check_full;
                flat.SleepPlaces = check_sleep;
                flat.Price = check_price;

                return await Services.FlatService.PutKv(jwtdata, flat);
            }
            else return "400 - Bad request";
        }
    }
}
