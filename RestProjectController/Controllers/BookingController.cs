using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;

namespace RestProjectController.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        public string Index()
        {
            return "booking xd";
        }
        [HttpGet("all")]
        public async Task<string> Get() => await Services.BookingService.Get();

        [HttpGet("{id}")]
        public async Task<string> GetByIndex(string id) => await Services.BookingService.GetByID(id);

        [HttpPost("Add/{flat_id}/{date}:{days}"), Authorize]
        public async Task<string> Reserve(string flat_id, [FromHeader(Name = "Authorization")] string jwt, string date, string days)
        {
            string username = Services.AuthService.GetNameJWT(jwt);
            return await Services.BookingService.Reserve(ObjectId.Parse(flat_id), username, DateTime.Parse(date), days);
        }
        [HttpPatch("Cancel:{id}"), Authorize]
        public async Task<string> Cancel(string id, [FromHeader(Name = "Authorization")] string jwt)
        {
            string username = Services.AuthService.GetNameJWT(jwt);
            return await Services.BookingService.Cancel(username, ObjectId.Parse(id));
        }
    }
}