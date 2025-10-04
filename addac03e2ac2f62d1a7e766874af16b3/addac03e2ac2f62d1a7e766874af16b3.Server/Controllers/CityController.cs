using addac03e2ac2f62d1a7e766874af16b3.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace addac03e2ac2f62d1a7e766874af16b3.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController(AppDbContext context) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<City?> GetCity(int id)
        {
            return await context.Cities.FindAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<City>> AddCity([FromBody] CityBase cityData)
        {
            var city = new City { Name = cityData.Name };
            context.Cities.Add(city);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCity), new { id = city.Id }, city);
        }
    }
}
