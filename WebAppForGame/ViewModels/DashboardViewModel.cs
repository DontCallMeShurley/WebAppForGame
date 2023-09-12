using WebAppForGame.Data;

namespace WebAppForGame.ViewModels
{
    public class DashboardViewModel
    {
        public List<log_gameover> log_Gameovers { get; set; }

        public int TotalUsers { get; set; }

        public double TotalPaid { get; set; }

        public int TotalLoginPerDay { get; set; }
        public long MaxPoints { get; set; }
        public int TotalGameOversPerDay { get; set; }



    }
}
