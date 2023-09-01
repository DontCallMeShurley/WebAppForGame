using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebAppForGame.Data
{
    public class log_gameover
    {
        [Key]
        public Guid id { get; set; }
        public long time { get; set; }
        public string user_id { get; set; }
        public long score { get; set; }
    }
}
