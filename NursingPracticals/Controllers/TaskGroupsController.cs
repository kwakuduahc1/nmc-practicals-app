using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NursingPracticals.Contexts;
using NursingPracticals.Mappers;
using NursingPracticals.Models;
using System.Collections;

namespace NursingPracticals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskGroupsController(DbContextOptions<ApplicationDbContext> options, CancellationToken token) : ControllerBase
    {
        private readonly ApplicationDbContext db = new(options);

        [HttpGet]
        public async Task<IEnumerable> List()
        {
            return await db.TaskGroups.Select(x => new
            {
                x.GroupName,
                x.TaskGroupsID,
                x.Programs
            }).ToListAsync();

        }

        [HttpGet("{id:int:required}")]
        public async Task<IActionResult> Find(int id)
        {
            var prog = await db.TaskGroups.FindAsync(id);
            return prog is null ? NotFound(new { Message = "The program was not found" }) : Ok(prog);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTaskGroupsModel task)
        {
            var p = new TaskGroupMapper().AddTask(task);
            db.TaskGroups.Add(p);
            await db.SaveChangesAsync(token);
            return Ok(p);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditTaskGroupsModel task)
        {
            var p = await db.TaskGroups.FindAsync(task.TaskGroupsID);
            p.GroupName = task.GroupName;
            p.Programs = task.Programs;
            db.Entry(p).State = EntityState.Modified;
            await db.SaveChangesAsync(token);
            return Accepted();
        }
    }
}
