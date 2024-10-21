using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NursingPracticals.Contexts;
using System.Collections;


namespace NursingPracticals.Controllers
{
    [EnableCors("bStudioApps")]
    [Route("api/[controller]")]
    [Authorize(Policy = "Administrators")]
    [ApiController]
    public class UsersController(DbContextOptions<ApplicationDbContext> contextOptions, CancellationToken token ) : ControllerBase
    {
        private readonly ApplicationDbContext db = new(contextOptions);


        [HttpGet]
        public async Task<IEnumerable> List()
        {
            var qry = """
                SELECT u.id, u.username, u.fullname, c.claimvalue "role"
                FROM "AspNetUsers" u
                INNER JOIN LATERAL(
                    SELECT  c.userid, c.claimvalue
                    FROM "AspNetUserClaims" c
                    WHERE c.userid = u.id
                    ORDER BY id 
                    LIMIT 1
                ) c ON c.userid = u.id;
                """;
            return db.Database.GetDbConnection().Query<UsersDetails>(qry);
        }
    }

    public record UsersDetails(string ID, string UserName, string FullName, string Role);
}