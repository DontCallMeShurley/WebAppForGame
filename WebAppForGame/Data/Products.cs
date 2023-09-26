
using System.ComponentModel.DataAnnotations;

namespace WebAppForGame.Data
{
    public class Products
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Стоимость")]
        public double Amount { get; set; }

        [Display(Name = "Наименование")]
        public string? Name { get; set; }

        [Display(Name = "Попытки")]
        public int Coins { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }
    }
}
