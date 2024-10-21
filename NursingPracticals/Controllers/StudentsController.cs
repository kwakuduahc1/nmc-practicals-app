using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NursingPracticals.Contexts;
using NursingPracticals.Mappers;
using NursingPracticals.Models;
using System.Collections;
using System.Collections.Generic;

namespace NursingPracticals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Administrators")]
    [EnableCors("bStudioApps")]
    public class StudentsController(DbContextOptions<ApplicationDbContext> options, CancellationToken token) : ControllerBase
    {
        private readonly ApplicationDbContext db = new(options);

        [HttpGet("List/{id:int:required}")]
        public async Task<IEnumerable> List(int id)
        {
            return await db.Students.Where(x => x.MainClassesID == id && x.IsActive)
                .Select(x => new
                {
                    x.StudentsID,
                    x.IndexNumber,
                    x.FullName,
                }).ToArrayAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddStudentModel[] students)
        {
            if (students.Length == 0)
                return BadRequest(new { Message = "Empty students list" });
            var ids = await db.Database.GetDbConnection().QueryAsync("""
                SELECT indexnumber 
                FROM students 
                WHERE indexnumber = ANY(ARRAY[@ids]);
                """, new { ids = students.Select(x => x.IndexNumber).ToArray() });
            if (ids.Any())
                return BadRequest(new { ids });
            List <Students> stds = new(students.Length);
            var map = new StudentsMapper();
            foreach (var std in students)
                stds.Add(map.FromAddModel(std));
            stds.ForEach(x=>x.IsActive = true);
            db.Students.AddRange(stds);
            await db.SaveChangesAsync(token);
            return Ok(stds);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditStudentModel std)
        {
            var s = await db.Students.FindAsync(std.StudentID);
            s.IndexNumber = std.IndexNumber;
            s.FullName = std.FullName;
            db.Entry(s).State = EntityState.Modified;
            await db.SaveChangesAsync(token);
            return Accepted();
        }
    }
}
