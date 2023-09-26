using System.ComponentModel.DataAnnotations;
namespace WebAppForGame.Data
{
    public class userid_mapping
    {
        [Key]
        public Guid id { get; set; }

        [Display(Name = "ID пользователя")]
        public string? user_id { get; set; }

        [Display(Name = "Внутренний ID")]
        public string? mapped_id { get; set; }
    }
}
