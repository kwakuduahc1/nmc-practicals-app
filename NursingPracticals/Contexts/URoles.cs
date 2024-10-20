﻿using System.ComponentModel.DataAnnotations;

namespace NursingPracticals.Contexts
{
    public class URoles
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public required string Role { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 30)]
        public required string ID { get; set; }
    }
}