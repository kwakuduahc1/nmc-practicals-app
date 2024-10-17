using Dapper;
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
    public class TasksController(DbContextOptions<ApplicationDbContext> options, CancellationToken token) : ControllerBase
    {
        private readonly ApplicationDbContext db = new(options);

        [HttpGet("{id:int:required}")]
        public async Task<IActionResult> Find(int id)
        {
            var task = await db.ComponentTasks.FindAsync(id, token);
            return task is null ? NotFound(new { Message = "The task was not found" }) : Ok(task);
        }

        [HttpGet("Students/{id:int:required}")]
        public async Task<IEnumerable> Details(int id)
        {
            var qry = """
                SELECT t.componenttask, t.componenttasksid, s.stepsid, s.stepname
                FROM componenttasks t 
                INNER JOIN steps s ON s.componenttasksid = t.componenttasksid
                WHERE t.componenttasksid = @id AND t.active = True                
                """;
            var res = await db.Database.GetDbConnection().QueryAsync<TaskSteps>(qry, new { id });
            if(res.Any())
            return res.GroupBy(x=> new { x.ComponentTask, x.ComponentTasksID }, (k,v)=> new
            {
                k.ComponentTasksID,
                k.ComponentTask ,
                Steps = v.Select(x=> new
                {
                    x.StepsID,
                    x.StepName,
                })
            }).AsEnumerable();
            return Enumerable.Empty<TaskSteps>();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddComponentTasks task)
        {
            var p = new ComponentsMapper().AddTasks(task);
            db.ComponentTasks.Add(p);
            await db.SaveChangesAsync(token);
            return Ok(p);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditComponentTaskModel mainClass)
        {
            var p = await db.ComponentTasks.FindAsync(mainClass.ComponentTasksID);
            p.ComponentTask = mainClass.ComponentTask;
            p.IsActive = true;
            db.Entry(p).State = EntityState.Modified;
            await db.SaveChangesAsync(token);
            return Accepted();
        }
    }

    public record TaskSteps(string ComponentTask, int ComponentTasksID, int StepsID, string StepName);
}
