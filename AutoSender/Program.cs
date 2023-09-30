using AutoSender;
using EFCoreDockerMySQL;
using Microsoft.EntityFrameworkCore;
using System;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await SendEmail();
        }

        private static async Task SendEmail()
        {
            Console.WriteLine("Here " + DateTime.Now);
            while (true)
            {
                await TaskWaiter.WaitUntil(0, 0).ContinueWith(task =>
                {
                    Console.WriteLine("Start get lists");
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        var currDay = DateTime.Today;
                        var listOfPlaying = db.log_gameover.Where(x => x.Date < currDay && x.Date >= currDay.AddDays(-1)).ToList();
                        Console.WriteLine($"found {listOfPlaying.Count}. Go Send");
                        EmailSender.SendEmail(listOfPlaying);

                    }
                });

            }
        }
    }
}