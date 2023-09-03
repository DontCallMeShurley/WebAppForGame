using Microsoft.EntityFrameworkCore;
using System;
using WebAppForGame.Data;

namespace EFCoreDockerMySQL
{

    public class ApplicationDbContext : DbContext
    {
        public DbSet<log_gameover> log_gameover { get; set; }
        public DbSet<userid_mapping> userid_mapping { get; set; }
        public DbSet<userlog_in> userlog_in { get; set; }
        public DbSet<SerialNumbers> SerialNumbers { get; set; }

        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=65.109.67.134;userid=u10_MwWuFyqu9H;pwd=Nzyib.gJMqjhMvWPXJ+x9roG;port=4000;database=s10_speedbox;sslmode=none;",
                new MySqlServerVersion(new Version(8, 0, 11))
            );
        }
    }
}