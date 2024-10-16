using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NursingPracticals.Contexts;
using NursingPracticals.Models;

namespace NursingPracticals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StepsController(DbContextOptions<ApplicationDbContext> options, CancellationToken token) : ControllerBase
    {
        private readonly ApplicationDbContext db = new(options);

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditStepsModel step)
        {
            var s = await db.Steps.FindAsync(step.StepsID, token);
            if (s is null)
                return BadRequest(new { Message = "The step was not found" });
            s.StepName = step.StepName;
            db.Entry(s).State = EntityState.Modified;
            await db.SaveChangesAsync(token);
            return Accepted();
        }

        [HttpPost("AddStep")]
        public async Task<IActionResult> AddStep([FromBody] AddFullStepsModel step)
        {
            var s = new Steps { StepName = step.StepName, ComponentTasksID = step.ComponentTasksID };
            db.Steps.Add(s);
            await db.SaveChangesAsync(token);
            return Ok(s);
        }

        [HttpDelete("{id:int:required}")]
        public async Task<IActionResult> Delete(int id)
        {
            var step = await db.Steps.FindAsync(id, token);
            db.Entry(step).State = EntityState.Deleted;
            await db.SaveChangesAsync(token);
            return Accepted();
        }
    }
}
