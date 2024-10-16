using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NursingPracticals.Contexts
{
    public class ApplicationUsers : IdentityUser
    {
        [StringLength(64, MinimumLength = 36)]
        public override string Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public override required string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public required string FullName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6)]
        [NotMapped]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6)]
        [NotMapped]
        [Compare(nameof(Password), ErrorMessage = "Both passwords must match")]
        public string? ConfirmPassword { get; set; }
    }

    public class ApplicationRoles : IdentityRole;
}