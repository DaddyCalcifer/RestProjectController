using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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
        public async Task<string> Get() => await Models.Reservation.Get();

        [HttpGet("{id}")]
        public async Task<string> GetByIndex(string id) => await Models.Reservation.GetByID(id);

        [HttpPost("Add/{flat_id}:{owner_id}/{date}:{days}")] //пока get-запрос для проверки и тестового заполнения бд, потом перепишу его в post
        public async Task<string> Reserve(string flat_id, string owner_id, string date, string days) => await Models.Reservation.Reserve(ObjectId.Parse(flat_id), ObjectId.Parse(owner_id), DateTime.Parse(date), days);
    }
}