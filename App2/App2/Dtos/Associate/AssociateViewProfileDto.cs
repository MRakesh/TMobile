using App2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace App2.Associates.Dtos
{
    public class AssociateViewProfileDto
    {
        public int AssociateId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postal { get; set; }
        public string GeoArea { get; set; }
        public DateTime? OptStDate { get; set; }
        public DateTime? OptEndDate { get; set; }
        public List<string> Auths { get; set; }
        public bool? IsRelocate { get; set; }
        public string Availability { get; set; }
        public string AnnualSalary { get; set; }
        public string DesiredPosition { get; set; }
        public string Experience { get; set; }
        public string ContractType { get; set; }
    }
    public class AssociateViewProfileDtoBase : BaseAPIResponseModel
    {
        public AssociateViewProfileDto result { get; set; }
    }
}
