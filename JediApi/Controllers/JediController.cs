using JediApi.Models;
using JediApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace JediApi.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class JediController(JediService jediService) : ControllerBase
    {
        private readonly JediService _jediService = jediService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJedi(int id)
        {
            var jedi = await _jediService.GetByIdAsync(id);

            if (jedi == null)
            {
                return NotFound();
            }

            return Ok(jedi);
        }

        [HttpGet]
        public async Task<ActionResult<List<Jedi>>> GetJedis()
        {
            return await _jediService.GetAllAsync();
        }

        [HttpPost]
        public async Task<IActionResult> CreateJedi(Jedi jedi)
        {
            var newJedi = await _jediService.AddAsync(jedi);
            return Created(nameof(GetJedi), newJedi);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJedi(int id, Jedi jedi, [FromHeader(Name = "If-Match")] int ifMatch)
        {
            var existingJedi = await _jediService.GetByIdAsync(id);
            if (existingJedi == null)
            {
                return NotFound();
            }

            if (existingJedi.Version != ifMatch)
            {
                return Conflict();
            }

            jedi.Id = id;
            jedi.Version += 1;

            var success = await _jediService.UpdateAsync(jedi);
            if (!success)
            {
                return NotFound();
            }

            return Ok(jedi);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJedi(int id)
        {
            var success = await _jediService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
