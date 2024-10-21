using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NursingPracticals.Contexts;
using NursingPracticals.Mappers;
using NursingPracticals.Models;
using System.Collections;

namespace NursingPracticals.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    [EnableCors("bStudioApps")]
    [Authorize(Policy = "Administrators")]
    public class ProgramsController(DbContextOptions<ApplicationDbContext> options, CancellationToken token) : ControllerBase
    {
        private readonly ApplicationDbContext db = new(options);

        [HttpGet]
        public async Task<IEnumerable> List()
        {
            var qry = """
                SELECT p.programsid, p.programname, m.mainclassesid, m.classname 
                FROM programs p 
                FULL JOIN(
                    SELECT m.mainclassesid, m.classname, m.programsid
                    FROM mainclasses m
                    WHERE m.classstatus = True
                ) m ON m.programsid = p.programsid
                """;
            var prog = await db.Database.GetDbConnection().QueryAsync<ProgramsVM>(qry);
            return prog.GroupBy(x => new { x.ProgramsID, x.ProgramName }, (k, v) => new
            {
                k.ProgramsID,
                k.ProgramName,
                Classes = v.Select(x => new { x.ClassName, x.MainClassesID })
            }).AsEnumerable();
        }

        [HttpGet("{id:int:required}")]
        public async Task<IActionResult> Find(int id)
        {
            var prog = await db.Programs.FindAsync(id);
            return prog is null ? NotFound(new { Message = "The program was not found" }) : Ok(prog);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddProgramModel prog)
        {
            var p = new ProgramsMapper().AddProgram(prog);
            db.Programs.Add(p);
            await db.SaveChangesAsync(token);
            return Ok(p);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditProgramModel prog)
        {
            var p = await db.Programs.FindAsync(prog.ProgramsID);
            p.ProgramName = prog.ProgramName;
            db.Entry(p).State = EntityState.Modified;
            await db.SaveChangesAsync(token);
            return Accepted();
        }
    }

    public record ProgramsVM(int ProgramsID, string ProgramName, int MainClassesID, string ClassName);
}
