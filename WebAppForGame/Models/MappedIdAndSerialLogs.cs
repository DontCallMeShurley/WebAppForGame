using System.ComponentModel.DataAnnotations;

namespace WebAppForGame.Models
{
    public class MappedIdAndSerialLogs
    {
        public Guid id { get; set; }

        [Display(Name = "Игрок")]
        public string userId { get; set; }

        [Display(Name = "Внутренний ID")]
        public string mappedId { get; set; }

        [Display(Name = "Серийный номер")]
        public string serialNumber { get; set; }

        [Display(Name = "Порядковый номер")]
        public string number { get; set; }
    }
}
