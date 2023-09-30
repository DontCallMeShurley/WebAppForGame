using System.ComponentModel.DataAnnotations;

namespace WebAppForGame.Data
{
   
    public class Settings
    {
        [Key]
        public int Id { get; set; }
        public string? BearerToken { get; set; }
        public string? MerchantId { get; set; }
        public string? AdminPassword { get; set; }

    }
}
