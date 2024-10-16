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
    public class ClassesController(DbContextOptions<ApplicationDbContext> options, CancellationToken token) : ControllerBase
    {
        private readonly ApplicationDbContext db = new(options);

        [HttpGet("{id:int:required}")]
        public async Task<IActionResult> Find(int id)
        {
            var classes = await db.MainClasses.FindAsync(id);
            return classes is null ? NotFound(new { Message = "The class was not found" }) : Ok(classes);
        }

        //[HttpGet("Students/{id:int:required}")]
        //public async Task<IEnumerable> Details(int id)
        //{
        //    var qry = """
        //        SELECT s.studentid, s.indexnumber, s.mainclassesid, s.fullname
        //        FROM students s 
        //        WHERE s.mainclassesid = @id AND s.isactive = False;
        //        SELECT s.fullname, s.indexnumber, s.mainclassesid, s.studentid
        //        FROM students s 
        //        WHERE s.mainclassesid = @id AND s.isactive = False
        //        """;
        //    var res = await db.Database.GetDbConnection().QueryMultipleAsync(qry, new { id });

        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddMainClassModel addMain)
        {
            var p = new ClassesMappers().AddClass(addMain);
            db.MainClasses.Add(p);
            await db.SaveChangesAsync(token);
            return Ok(p);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditMainClassModel mainClass)
        {
            var p = await db.MainClasses.FindAsync(mainClass.ProgramsID);
            p.ClassName = mainClass.ClassName;
            p.ClassStatus = true;
            db.Entry(p).State = EntityState.Modified;
            await db.SaveChangesAsync(token);
            return Accepted();
        }
    }
}
