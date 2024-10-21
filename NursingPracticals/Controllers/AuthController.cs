using EduApp.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NursingPracticals.Contexts;
using NursingPracticals.Controllers.Helpers;
using NursingPracticals.Models.AuthVm;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;


namespace NursingPracticals.Controllers
{
    [EnableCors("bStudioApps")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<ApplicationUsers> userManager, SignInManager<ApplicationUsers> signInManager, DbContextOptions<ApplicationDbContext> contextOptions, IWebHostEnvironment environment, IAppFeatures app) : ControllerBase
    {
        private readonly UserManager<ApplicationUsers> _userManager = userManager;
        private readonly SignInManager<ApplicationUsers> _signInManager = signInManager;
        public IWebHostEnvironment Env { get; } = environment;
        private readonly ApplicationDbContext db = new(contextOptions);


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginVm user)
        {
            var _user = await _userManager.FindByNameAsync(user.UserName);
            if (_user == null)
                return Unauthorized(new { Message = "Invalid user name or password" });
            if (!await _userManager.CheckPasswordAsync(_user, user.Password))
                return Unauthorized();
            await _signInManager.SignInAsync(_user, false);
            var claims = await _userManager.GetClaimsAsync(_user);
            var token = new AuthHelper(claims, app).GetKey();
            return Ok(new { Token = token });
        }

        [HttpPost("Register")]
        //[Authorize(Policy = "Administrators")]
        public async Task<IActionResult> Register([FromBody] RegisterVm reg, CancellationToken token)
        {
            if (reg.Password != reg.ConfirmPassword)
                return BadRequest(new { Error = "The confirmation password must match" });
            ApplicationUsers user = new RegisterMapper().ToUser(reg);
            var result = await _userManager.CreateAsync(user, user.Password);
            if (!result.Succeeded)
                return BadRequest(new { Message = result.Errors.First().Description });
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, reg.Role));
            await _userManager.AddClaimAsync(user, new Claim("UsersID", user.Id));
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Name, user.UserName));
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "User"));
            //await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, reg.Email));
            await _userManager.AddClaimAsync(user, new Claim("FullName", reg.FullName));
            //await _userManager.SetEmailAsync(user, reg.Email);
            user.EmailConfirmed = true;
            user.LockoutEnabled = false;
            await db.SaveChangesAsync(token);
            return Created("", new { user.UserName, user.Id });
        }

       

        [HttpPut("SetPassword")]
        public async Task<IActionResult> SetPassword([FromBody] PasswordHack user)
        {
            var _user = await _userManager.FindByNameAsync(user.UserName);
            await _userManager.AddPasswordAsync(_user, user.Password);
            // await _userManager.AddClaimAsync(_user, new Claim(ClaimTypes.Role, "Student"));
            return Ok();
        }

        [HttpPost("SignOut")]
        [Authorize]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Accepted();
        }
    }

    public class PasswordHack
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}