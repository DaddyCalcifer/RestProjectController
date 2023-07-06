using Microsoft.AspNetCore.Mvc;

namespace RestProjectController.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthCheckController : ControllerBase
    {
        public string Index() => "204 - No Content";
    }
}
