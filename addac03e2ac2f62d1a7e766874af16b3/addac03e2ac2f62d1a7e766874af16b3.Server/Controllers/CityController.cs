using addac03e2ac2f62d1a7e766874af16b3.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace addac03e2ac2f62d1a7e766874af16b3.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController(AppDbContext context) : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<City?>> GetCity(long id)
        {
            var city = await context.Cities.FindAsync(id);
            return city == null ? NotFound(new { message = $"City with ID {id} not found." }) : Ok(city);
        }

        [HttpPost]
        public async Task<ActionResult<City>> AddCity([FromBody] CityBase cityData)
        {
            var city = new City 
            { 
                Name = cityData.Name,
                State = cityData.State,
                Country = cityData.Country,
                Rating = cityData.Rating,
                DateEstablished = cityData.DateEstablished,
                Population = cityData.Population,
            };
            context.Cities.Add(city);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCity), new { id = city.Id }, city);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCity(long id, [FromBody] UpdateCityDto dto)
        {
            var city = await context.Cities.FindAsync(id);
            if (city == null) return NotFound(new { message = $"City with ID {id} not found." });
            city.Rating = dto.Rating;
            city.DateEstablished = dto.DateEstablished;
            city.Population = dto.Population;
            await context.SaveChangesAsync();
            return Ok(new { message = "City updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(long id)
        {
            var city = await context.Cities.FindAsync(id);
            if (city == null) return NotFound(new { message = $"City with ID {id} not found." });
            context.Cities.Remove(city);
            await context.SaveChangesAsync();
            return Ok(new { message = $"City with ID {id} deleted successfully." });
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CityBase>>> SearchCity([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest(new { message = "City name is required." });

            var results = await context.Cities
            .Where(c => c.Name.Contains(name))
            .Select(c => new CityBase
            {
                Name = c.Name,
                State = c.State,
                Country = c.Country,
                Rating = c.Rating,
                DateEstablished = c.DateEstablished,
                Population = c.Population
            })
            .ToListAsync();

            if (results.Count == 0) return NotFound(new { message = $"No cities found matching '{name}'." });
            return Ok(results);
        }
    }
}
