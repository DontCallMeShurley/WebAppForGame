using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebAppForGame.Data
{
    public class SerialNumbers
    {
        [Key]
        public Guid id { get; set; }
        public string user_id { get; set; }
        public string serial_number { get; set; }
    }
}
