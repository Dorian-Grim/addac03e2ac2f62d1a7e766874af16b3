using addac03e2ac2f62d1a7e766874af16b3.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Text.Json;

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
        public async Task<ActionResult<City>> AddCity([FromBody] City cityData)
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
        public async Task<ActionResult<IEnumerable<City>>> SearchCity([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest(new { message = "City name is required." });

            var results = await context.Cities
            .Where(c => c.Name.Contains(name))
            .Select(c => new City
            {
                Id = c.Id,
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

        [HttpGet("restcountries")]
        public IActionResult GetCountries([FromQuery] string cityName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "CountryData", "countries.json");
            if (!System.IO.File.Exists(filePath)) return NotFound(new { message = "Countries data not found." });
            var json = System.IO.File.ReadAllText(filePath);
            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};
            var countries = JsonSerializer.Deserialize<List<JsonElement>>(json, options)!;
            if (string.IsNullOrWhiteSpace(cityName)) return Ok(countries);
            var filtered = countries.Where(c => c.TryGetProperty("capital", out var nameProp) && nameProp.GetString()?.Contains(cityName, StringComparison.OrdinalIgnoreCase) == true).ToList();
            return filtered.Count == 0 ? NotFound(new { message = $"Did not find any country with city '{cityName}'." }) : Ok(filtered) ;
        }
    }
}
