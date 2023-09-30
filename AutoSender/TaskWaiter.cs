using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSender
{
    static class TaskWaiter
    {
        /// <summary>
        /// пауза до указанного часа и минуты
        /// </summary>
        /// <param name="hour">час в формате 24</param>
        /// <param name="minute"></param>
        internal static async Task WaitUntil(int hour, int minute)
        {
            DateTime now = DateTime.Now;
            DateTime until = new DateTime(now.Year, now.Month, now.Day, hour, minute, 59);
            while (until < now)
            {
                until = until.AddDays(1);
            }
            Console.WriteLine($"Delay to {until - now}");
            await Task.Delay(until - now);
        }

    }
}
