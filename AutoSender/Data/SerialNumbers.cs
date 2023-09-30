using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebAppForGame.Data
{
    public class SerialNumbers
    {
        [Key]
        public Guid id { get; set; }

        [Display(Name ="ID пользователя")]
        public string? user_id { get; set; }

        [Display(Name = "Серийный номер")]
        public string? serial_number { get; set; }
    }
}
