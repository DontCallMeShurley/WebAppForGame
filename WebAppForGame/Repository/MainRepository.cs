﻿using DevExpress.Data.Linq.Helpers;
using EFCoreDockerMySQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Web.Administration;
using Newtonsoft.Json;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using WebAppForGame.Data;
using WebAppForGame.ViewModels;
using System.Configuration;

namespace WebAppForGame.Repository
{
    public class MainRepository
    {
        private readonly ApplicationDbContext _context;

        public MainRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public DashboardViewModel GetViewModel()
        {
            DashboardViewModel dashboard = new DashboardViewModel();

            var currentStartDay = getStartDayTimestamp();


            var gameovers = _context.log_gameover.AsNoTracking().OrderByDescending(x => x.time).ToList();
            var userIdMapping = _context.userid_mapping.AsNoTracking().ToList();

            gameovers.ForEach(x => x.user_id = userIdMapping.FirstOrDefault(y => y.user_id == x.user_id)?.mapped_id ?? x.user_id);


            dashboard = new DashboardViewModel()
            {
                log_Gameovers = gameovers,
                TotalLoginPerDay = _context.userlog_in.Count(x => x.time > currentStartDay),
                TotalPaid = 180,
                TotalUsers = userIdMapping.Count(),
                MaxPoints = gameovers.Max(x => x.score),
                TotalGameOversPerDay = gameovers.Count(x => x.time > currentStartDay)
            };

            return dashboard;
        }

        public async Task<string> GetPayLink(string userID, int productID)
        {
            using (HttpClient _httpClient = new HttpClient())
            {

                var product = _context.Products.FirstOrDefault(x => x.Id == productID);
                var user = _context.userid_mapping.FirstOrDefault(x => x.mapped_id == userID);

                if (product == null || user == null)
                    throw new Exception($"Неправильный формат. Не найден {(user == null ? "Пользователь" : "Продукт")}");

                var payment = new Payments()
                {
                    Product = product,
                    UserID = user.mapped_id
                };

                var settings = await _context.Settings.FirstAsync();

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", settings.BearerToken);

                var paymentRequest = new
                {
                    merchantId = settings.MerchantId,
                    testMode = true,
                    invoice = new
                    {
                        description = product.Name
                    },
                    amount = new
                    {
                        value = product.Amount,
                        currency = "RUB"
                    },
                    customer = new
                    {
                        account = userID
                    },
                    paymentMethod = "bankcard"
                };

                string json = JsonConvert.SerializeObject(paymentRequest);

                _httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(System.Configuration.ConfigurationManager.AppSettings["PaymentUrl"], new StringContent(json, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();
                var responseObject = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(responseBody) ?? new Dictionary<string, string>();

                string paymentId = responseObject["paymentId"];
                payment.PaymentId = paymentId;
                _context.Payments.Add(payment);
                Save();

                string url = responseObject["url"];

                return url;
            }
        }
        
        private async void Save()
        {
            await _context.SaveChangesAsync();
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