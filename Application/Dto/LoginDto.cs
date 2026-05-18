using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Dto
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string EmailAdress { get; set; }


        [Required]
        public string Password { get; set; }
    }
}
