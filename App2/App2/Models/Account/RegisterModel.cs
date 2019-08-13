using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App2.Models.Account
{
    public class RegisterModel
    {
        [Required, MaxLength(50), EmailAddress]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
