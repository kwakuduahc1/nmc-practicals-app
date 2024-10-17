using Dapper;
using Microsoft.AspNetCore.Http;
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
    public class StudentsController(DbContextOptions<ApplicationDbContext> options, CancellationToken token) : ControllerBase
    {
        private readonly ApplicationDbContext db = new(options);

        [HttpGet("List/{id:int:required}")]
        public async Task<IEnumerable> List(int id)
        {
            return await db.Students.Where(x => x.MainClassesID == id && x.IsActive)
                .Select(x => new
                {
                    x.StudentID,
                    x.IndexNumber,
                    x.FullName,
                }).ToArrayAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddStudentModel[] students)
        {
            for (int i = 0; i < students.Length; i++)
                db.Add(new StudentsMapper().FromAddModel(students[i]));
            await db.SaveChangesAsync(token);
            return Ok();
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
