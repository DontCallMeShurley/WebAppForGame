using DevExpress.Xpo;

namespace WebAppForGame.Data
{
   
    public class Settings
    {
        [Key]
        public int Id { get; set; }
        public string? BearerToken { get; set; }
        public string? MerchantId { get; set; }

    }
}
