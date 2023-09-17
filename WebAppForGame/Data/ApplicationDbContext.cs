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
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Settings> Settings { get; set; }

        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}