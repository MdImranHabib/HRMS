using System;
using System.ComponentModel.DataAnnotations;

namespace HRMS.Models
{
    public class Rent
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Flat")]
        public int FlatId { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name ="Rent Month")]
        public string RentMonth { get; set; }

        [Required]
        [Display(Name = "Flat Rent")]
        public double FlatRent { get; set; }

        [Required]
        [Display(Name = "Electric Bill")]
        public double ElectricBill { get; set; }

        [Required]
        [Display(Name = "Gas Bill")]
        public double GasBill { get; set; }

        [Required]
        [Display(Name = "Water Bill")]
        public double WaterBill { get; set; }

        [Required]
        [Display(Name = "Total Bill")]
        public double TotalBill { get; set; }

        [Required]     
        public double Paid { get; set; }

        [Required]
        [Display(Name ="Billing Date")]
        public DateTime Date { get; set; }

        [StringLength(200)]
        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        public virtual Flat Flat { get; set; }
    }
}
