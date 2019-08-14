using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App2.Models
{
    public class RegisterModel
    {
        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        [Required]
        public string Password { get; set; }

        public string GUID { get; set; }
    }

    public class RegisterAutomateModel
    {
        public bool automatic { get; set; }
        public string msg { get; set; }
    }
    public class RegisterAutomateModelBase : BaseAPIResponseModel
    {
        public RegisterAutomateModel result { get; set; }
    }
}
