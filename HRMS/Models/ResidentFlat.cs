using System;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class ResidentFlat
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Resident")]
        public int ResidentId { get; set; }

        [Required]
        [Display(Name = "Flat")]
        public int FlatId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Arrival Date")]
        public DateTime ArrivalDate { get; set; }
    
        [DataType(DataType.Date)]
        [Display(Name = "Departure Date")]
        public DateTime? DepartureDate { get; set; }

        public bool Status { get; set; }

        public virtual Resident Resident { get; set; }
        public virtual Flat Flat { get; set; }
    }
}
