using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class Resident
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Resident's Name")]
        public string Name { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [StringLength(15, MinimumLength = 11)]
        [Display(Name = "Contact No.")]
        public string ContactNo { get; set; }

        [Required]
        [StringLength(17)]
        [Display(Name ="NID No.")]
        public string NIDNo { get; set; }

        [Display(Name ="Resident's Photo")]
        public string PhotoPath { get; set; }

        [Required]
        [StringLength(100)]
        public string Occupation { get; set; }

        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        [Display(Name="Previous Address")]
        public string PrevAddress { get; set; }

        [Display(Name= "Opening Balance")]
        public double OpeningBalance { get; set; }
    }
}
