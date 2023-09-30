using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppForGame.Data
{
    public class log_gameover
    {
        [Key]
        public Guid id { get; set; }

        [Display(Name ="Дата игры")]
        public DateTime Date { get; set; }

        [Display(Name = "Пользователь")]
        public string? user_id { get; set; }

        [Display(Name = "Счёт")]
        public long score { get; set; }
    }
}
