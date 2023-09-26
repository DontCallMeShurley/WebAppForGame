using System.ComponentModel.DataAnnotations;
namespace WebAppForGame.Data
{
    public class userlog_in
    {
        [Key]
        public Guid id { get; set; }

        [Display(Name = "Дата входа")]
        public DateTime Date { get; set; }

        [Display(Name = "Пользователь")]
        public string? user_id { get; set; }
    }
}
