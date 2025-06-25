using Microsoft.AspNetCore.Mvc;

namespace MyWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var weather = new
            {
                Temperature = 29,
                Summary = "Cool weather",
                Humidity = 70
            };

            return Ok(weather);
        }
    }
}
