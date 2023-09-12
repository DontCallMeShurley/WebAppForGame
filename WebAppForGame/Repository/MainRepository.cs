using DevExpress.Data.Linq.Helpers;
using DevExpress.Xpo;
using EFCoreDockerMySQL;
using Microsoft.EntityFrameworkCore;
using WebAppForGame.Data;
using WebAppForGame.ViewModels;

namespace WebAppForGame.Repository
{
    public class MainRepository
    {

        public DashboardViewModel getViewModel()
        {
            DashboardViewModel dashboard = new DashboardViewModel();

            var currentStartDay = getStartDayTimestamp();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var gameovers = db.log_gameover.AsNoTracking().OrderByDescending(x => x.time).ToList();
                var userIdMapping = db.userid_mapping.AsNoTracking().ToList();

                gameovers.ForEach(x => x.user_id = userIdMapping.FirstOrDefault(y => y.user_id == x.user_id)?.mapped_id ?? x.user_id);


                dashboard = new DashboardViewModel()
                {
                    log_Gameovers = gameovers,
                    TotalLoginPerDay = db.userlog_in.Count(x => x.time > currentStartDay),
                    TotalPaid = 180,
                    TotalUsers = userIdMapping.Count(),
                    MaxPoints = gameovers.Max(x => x.score),
                    TotalGameOversPerDay = gameovers.Count(x => x.time > currentStartDay)
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
