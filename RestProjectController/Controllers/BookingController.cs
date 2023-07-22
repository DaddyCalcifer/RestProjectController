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

        [HttpPost("Add/{flat_id}/{fromDate}:{tillDate}"), Authorize]
        public async Task<string> Reserve(string flat_id, string fromDate, string tillDate, [FromHeader(Name = "Authorization")] string jwt)
        {
            var jwtdata = DataJWT.Parse(jwt);
            DateTime fromDate_parsed, tillDate_parsed; ObjectId flat_id_parsed;
            if (DateTime.TryParse(fromDate, out fromDate_parsed) && DateTime.TryParse(tillDate, out tillDate_parsed) && ObjectId.TryParse(flat_id, out flat_id_parsed))
            {
                Models.Reservation reserv = new Models.Reservation();
                reserv.flatId = flat_id_parsed;
                reserv.ClientID = jwtdata.Name;
                reserv.fromDate = fromDate_parsed;
                reserv.tillDate = tillDate_parsed;

                if(tillDate_parsed > fromDate_parsed)
                return await Services.BookingService.Reserve(reserv);
            }
            return "400 - Bad request";
        }
        [HttpPatch("Cancel:{id}"), Authorize]
        public async Task<string> Cancel(string id, [FromHeader(Name = "Authorization")] string jwt)
        {
            var data = DataJWT.Parse(jwt);
            return await Services.BookingService.Cancel(data, ObjectId.Parse(id));
        }
    }
}