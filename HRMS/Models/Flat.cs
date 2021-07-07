using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class Flat
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Flat Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Flat Category")]
        public string Category { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Flat Rent")]
        public double Rent { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Meter No")]
        public string MeterNo { get; set; }

        public bool Status { get; set; }
    }
}
