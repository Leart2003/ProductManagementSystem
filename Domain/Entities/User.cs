using Domain.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Entities
{
    public class User : IdentityUser
    {

        public int Id { get; set; }

        [Required]
        public string FirsName { get; set; }

        public string LastName { get; set; }


        [Phone]

        public string phoneNumber { get; set; }

        public string email { get; set; }


        public DateOnly  DateofBirth  { get; set; }

        public Gender Gender { get; set; }

        public string Password { get; set; }
    }
}
