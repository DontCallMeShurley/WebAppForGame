using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebAppForGame.Data
{
    public class UserNumber
    {
        [Key]
        public Guid id { get; set; }

        [Display(Name ="ID пользователя")]
        public string? user_id { get; set; }

        [Display(Name = "Порядковый номер")]
        public int? number { get; set; }
    }
}
