using System.ComponentModel.DataAnnotations;

namespace NursingPracticals.Models.AuthVm
{
    public class RegisterVm
    {
        [Required]
        [StringLength(30)]
        public required string UserName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 6)]
        public required string Password { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 6)]
        public required string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public required string FullName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(75, MinimumLength = 5)]
        public required string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [StringLength(30)]
        public required string PhoneNumber { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5)]
        [AllowedValues(["Tutor", "Student", "Administrator"])]
        public required string Role { get; set; }
    }
}
