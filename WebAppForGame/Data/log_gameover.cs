using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppForGame.Data
{
    public class log_gameover
    {
        [Key]
        public Guid id { get; set; }
        public long time { get; set; }
        public string user_id { get; set; }
        public long score { get; set; }

        public DateTime GetDateTime()
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(time).ToLocalTime();
            return dateTime;
        }
    }
}
