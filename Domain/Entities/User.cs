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

        

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }


        [Phone]

        public string PhoneNumber { get; set; } = string.Empty;

     


        public DateOnly  DateofBirth  { get; set; }

        public Gender Gender { get; set; }

        public List<Product> products = new List<Product>();

    
    }
}
