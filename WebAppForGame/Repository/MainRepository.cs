using DevExpress.Xpo;
using EFCoreDockerMySQL;
using WebAppForGame.Data;
using WebAppForGame.ViewModels;

namespace WebAppForGame.Repository
{
    public class MainRepository
    {

        public  DashboardViewModel getViewModel()
        {
            DashboardViewModel dashboard = new DashboardViewModel();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                dashboard = new DashboardViewModel()
                {
                    log_Gameovers = db.log_gameover.OrderByDescending(x => x.time).ToList(),
                    TotalLogin = db.userlog_in.Count(x => x.time > getStartDayTimestamp()),
                    TotalPaid = 180,
                    TotalUsers = db.userid_mapping.Count()
                };
            }
            return dashboard;
        }

        private long getStartDayTimestamp()
        {
            DateTime now = DateTime.Now;
            DateTime todayStartDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Local);
            long timestamp = (long)(todayStartDate - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            return timestamp;
        }
    }
}
