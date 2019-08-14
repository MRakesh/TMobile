using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App2.Models
{
    public class RegisterModel
    {
        public int PortalId { get; set; }
        public int CompanyId { get; set; }

        public int? JobId { get; set; }
        public int? RecruiterId { get; set; }   //For Recruiter Invitation of the Associates
        public long? AssociateRecruiterUserId { get; set; }   //For Associate Invitation of the Candidates


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
        public string FileName { get; set; }
        public string FileGUID { get; set; }
        public short? VerificationType { get; set; }

        //[Required]
        //public string FirstName { get; set; }

        //public string LastName { get; set; }

        //[Required]
        //public string Email { get; set; }
        //[Required]
        //public string PhoneNo { get; set; }
        //[Required]
        //public string Password { get; set; }

        //public string GUID { get; set; }
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
