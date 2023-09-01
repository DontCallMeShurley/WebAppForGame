using System.ComponentModel.DataAnnotations;
namespace WebAppForGame.Data
{
    public class userlog_in
    {
        [Key]
        public Guid id { get; set; }
        public long time { get; set; }
        public string user_id { get; set; }
    }
}
