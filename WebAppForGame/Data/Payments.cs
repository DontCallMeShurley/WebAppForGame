
using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppForGame.Data
{
    public class Payments 
    {
        [Key]
        public Guid Id { get; set; }

        public string? UserID { get; set; }
        public Products? Product { get; set; }
        public string? PaymentId { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime Date { get; set; }
    }

}