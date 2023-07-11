﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<string> Get() => await Models.Flats.Get();

        [HttpGet("ByName:{name}")]
        public async Task<string> GetByName(string name) => await Models.Flats.GetByName(name);
        [HttpGet("{id}")]
        public async Task<string> GetByIndex(string id) => await Models.Flats.GetByID(id);

        [HttpGet("BySleep:{count}")]
        public async Task<string> GetBySP(string count) => await Models.Flats.GetBySP(count);
        [HttpGet("ByFull:{val}")]
        public async Task<string> GetByFull(string val) => await Models.Flats.GetByFull(val);



        [HttpPost("Add/{name}/{full}/{sleep}/{cost}"), Authorize]
        public async Task<string> AddKv(string name, string full, string sleep, string cost, [FromHeader(Name = "Authorization")] string jwt)
        {
            string username = Models.Account.GetNameJWT(jwt);
            return await Models.Flats.AddKv(name, full, sleep, cost, username);
        }

        [HttpPatch("Delete:{id}"), Authorize]
        public async Task<string> DeleteKv(string id) => await Models.Flats.DeleteKv(id);

        [HttpPatch("Update:{id};{name};{full};{sleep};{cost}"), Authorize]
        public async Task<string> PutKv(string id, string name, string full, string sleep, string cost) => await Models.Flats.PutKv(id, name, full, sleep, cost);
    }
}
