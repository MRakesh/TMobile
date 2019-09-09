using System.ComponentModel.DataAnnotations;

namespace App2.Associates.Dtos
{
    public class CandidateAddResumeDto
    {
        public int ProfileId { get; set; }
        public int AssociateId { get; set; }

        [Required]
        [StringLength(256)]
        public string ProfileName { get; set; }

        [Required]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid Expected Candidate")]
        public decimal? ExpectedCandidateRate { get; set; }



        // [Required]
        public bool? IsWillingToRelocate { get; set; }
        public int? RateTypeId { get; set; }


        public string DesiredPosition { get; set; }

        public string Skills { get; set; }

        // [Required]
        public string[] SkillIds { get; set; }

        public string[] GeoRegionIds { get; set; }
        public string[] GeoStateIds { get; set; }


        public string[] DomainIds { get; set; }
        public string[] DesignationsIds { get; set; }

        public string FileName { get; set; }
        public string GUID { get; set; }
    }
}
