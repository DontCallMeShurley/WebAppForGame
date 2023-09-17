
using System.ComponentModel.DataAnnotations;

namespace WebAppForGame.Data
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        public double Amount { get; set; }
        public string? Name { get; set; }
        public int Coins { get; set; }
    }
}
