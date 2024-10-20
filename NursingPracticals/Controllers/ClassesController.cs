﻿using Dapper;
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
    public class ClassesController(DbContextOptions<ApplicationDbContext> options, CancellationToken token) : ControllerBase
    {
        private readonly ApplicationDbContext db = new(options);

        [HttpGet("{id:int:required}")]
        public async Task<IActionResult> Find(int id)
        {
            var classes = await db.MainClasses.FindAsync(id);
            return classes is null ? NotFound(new { Message = "The class was not found" }) : Ok(classes);
        }

        [HttpGet("Students/{id:int:required}")]
        public async Task<IActionResult> Details(int id)
        {
            var cls = await db.MainClasses.FindAsync(id);
            if (cls is null)
                return BadRequest(new { Message = "The class does not exist" });
            var qry = """
                SELECT s.fullname, s.indexnumber, s.mainclassesid, s.studentid
                FROM students s 
                WHERE s.mainclassesid = @id AND s.isactive = True;
                SELECT classschedulesid, schedulename, examdate, mainclassesid, classname, isactive
                FROM vw_class_schedules;
                SELECT taskgroupsid, groupname
                FROM taskgroups
                WHERE array_positions(programs, @pid) <> '{}'
                """;
            var res = await db.Database.GetDbConnection().QueryMultipleAsync(qry, new { id, pid = cls.ProgramsID });
            if (cls is null)
                return BadRequest(new { Message = "The class was not found" });
            var std = await res.ReadAsync<EditStudentModel>();
            var schs = await res.ReadAsync<ClassScheduleVm>();
            var groups = await res.ReadAsync<TaskGroupsVm>();
            return Ok(new { Class = new { cls.MainClassesID, cls.ProgramsID, cls.ClassName }, Students = std, Schedules = schs, groups });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddMainClassModel addMain)
        {
            var p = new ClassesMappers().AddClass(addMain);
            p.ClassStatus = true;
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
