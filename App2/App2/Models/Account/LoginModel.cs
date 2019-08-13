
using System.ComponentModel.DataAnnotations;
namespace App2.Models
{
    public class LoginModel
    {
        [Required, MaxLength(50), EmailAddress]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
    public class UserInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public short TypeId { get; set; }
        public int CompanyId { get; set; }
        public int AssociateOrRecruiterId { get; set; }
        public int UserPortalID { get; set; }
        public bool? IsDefaultContact { get; set; }
    }

    public class AngUserStatus 
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public UserInfo userInfo { get; set; }
    }
    public class AngUserStatusBase  : BaseAPIResponseModel
    {
        public AngUserStatus result { get; set; }
    }

}
