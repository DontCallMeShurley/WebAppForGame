
using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppForGame.Data
{
    public class Payments
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "ID пользователя")]
        public string? UserID { get; set; }

        [Display(Name = "Товар")]
        public Products? Product { get; set; }

        [Display(Name = "Номер заказа")]
        public string? PaymentId { get; set; }

        [Display(Name = "Статус платежа")]
        public string? PaymentStatus { get; set; }

        [Display(Name = "Дата совершения")]
        public DateTime Date { get; set; }
    }

}