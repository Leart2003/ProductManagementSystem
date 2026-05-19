using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Dto
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "The PhoneNumber field is not a valid phone number.")]
        public string PhoneNumber { get; set; } = string.Empty;


        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }


        [Required]
        public Gender Gender { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
            ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number, and special character.")]
        public string Password { get; set; }
    }
}
