
using System.ComponentModel.DataAnnotations;

namespace WebAppForGame.Data
{
    public class Log_GameStart
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Пользователь")]
        public string? UserID { get; set; }

        [Display(Name = "Дата и время")]
        public DateTime Date { get; set; }
    }
}
