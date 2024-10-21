using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NursingPracticals.Contexts;
using NursingPracticals.Mappers;
using NursingPracticals.Models;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace NursingPracticals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("bStudioApps")]
    //[Authorize(Policy = "Administrators")]
    public class ClassSchedulesController(DbContextOptions<ApplicationDbContext> options, CancellationToken token) : ControllerBase
    {
        private readonly ApplicationDbContext db = new(options);

        [HttpGet("Tutors/{id:required:int}")]
        public async Task<IActionResult> Tutors(int id)
        {
            string qry = """
                SELECT u.username, u.fullname
                FROM "AspNetUsers" u
                WHERE id IN (SELECT userid from "AspNetUserClaims"
                WHERE claimvalue = 'Tutor') 
                AND username NOT IN (SELECT tutor FROM teacherschedules WHERE classschedulesid = @_id);
                SELECT s.tutor username, u.fullname
                FROM teacherschedules s
                INNER JOIN "AspNetUsers" u ON s.tutor = u.username 
                WHERE classschedulesid = @_id
                """;
            using var con = db.Database.GetDbConnection();
            var res = await con.QueryMultipleAsync(qry, new { _id = id });
            var schTuts = await res.ReadAsync<Tutor>();
            var tuts = await res.ReadAsync<Tutor>();
            await con.CloseAsync();
            return Ok(new
            {
                Tutors = tuts,
                List = schTuts
            });
        }

        [HttpPost("Assign")]
        public async Task<IActionResult> Assign([FromBody] Assignments ass)
        {
            using var con = db.Database.GetDbConnection();
            await con.OpenAsync();
            var sch = await con.QueryFirstAsync<ClassSched>(
                """
                 SELECT mainclassesid, classname
                FROM vw_class_schedules
                WHERE classschedulesid = @id
                """, new { ass.ID });
            if (sch == null)
                return BadRequest(new { Message = "Schedule not found" });
            var stds = await con.QueryAsync<StudentDetails>("""
                SELECT studentsid, fullname, indexnumber
                FROM students
                WHERE mainclassesid = @id
                ORDER BY random()
                """, new { id = sch.MainClassesID});
            var take = stds.Count() / ass.Tutors.Length;
            var skip = 0;
            foreach (var t in ass.Tutors)
            {
                var list = stds.Skip(skip).Take(take)
                    .Select(x => new StudentsSchedules
                    {
                        FullName = x.FullName,
                        StudentsID = x.StudentsID,
                        IndexNumber = x.IndexNumber
                    }).ToList();
                var tlist = new TeacherSchedules
                {
                    ClassName = sch.ClassName,
                    Tutor = t,
                    ClassSchedulesID = ass.ID,
                    StudentsSchedules = list
                };
                db.TeacherSchedules.AddRange(tlist);
                skip += take;
            }
            await db.SaveChangesAsync(token);
            return Accepted();
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddClassScheduleModel schedule)
        {
            var sch = new ClassScheduleMappers().AddSchedule(schedule);
            db.ClassSchedules.Add(sch);
            await db.SaveChangesAsync(token);
            return Ok(sch.ClassSchedulesID);
        }

        [HttpPut]
        public async Task<IActionResult> Change([FromBody] int id)
        {
            var sch = await db.ClassSchedules.FindAsync(id, token);
            if (sch is null)
                return BadRequest(new { Message = "The schedule was not found" });
            sch.IsActive = !sch.IsActive;
            db.Entry(sch).State = EntityState.Modified;
            await db.SaveChangesAsync(token);
            return Accepted();
        }

        [HttpDelete("{id:int:required}")]
        public async Task<IActionResult> Delete(int id)
        {
            var step = await db.ClassSchedules.FindAsync(id, token);
            if (step == null)
                return BadRequest(new { Message = "The schedule was not found" });
            db.ClassSchedules.Remove(step);
            await db.SaveChangesAsync(token);
            return Accepted();
        }
    }

    public record Tutor(string UserName, string FullName);

    public record StudentDetails(int StudentsID, string FullName, string IndexNumber);

    public record ClassSched(int MainClassesID, string ClassName);
    public class Assignments
    {
        [Required]
        public required string[] Tutors { get; set; }

        [Required]
        public required int ID { get; set; }
    }
}
