using System;
using System.ComponentModel.DataAnnotations;


namespace HRMS.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int RentId { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]      
        public DateTime PaymentDate { get; set; }

        [Required]
        [StringLength(70)]
        public string Method { get; set; }

        public virtual Rent Rent { get; set; }
    }
}
