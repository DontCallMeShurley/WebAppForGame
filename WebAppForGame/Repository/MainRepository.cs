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
using System.Net.Http;
using NLog;

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

            var gameovers = _context.log_gameover.AsNoTracking().OrderByDescending(x => x.Date).ToList();
            var lastPayments = _context.Payments.AsNoTracking().Include(x => x.Product).OrderByDescending(x => x.Date).Take(3).ToList();
            var userIdMapping = _context.userid_mapping.AsNoTracking().ToList();

            gameovers.ForEach(x => x.user_id = userIdMapping.FirstOrDefault(y => y.user_id == x.user_id)?.mapped_id ?? x.user_id);


            dashboard = new DashboardViewModel()
            {
                log_Gameovers = gameovers,
                TotalLoginPerDay = _context.userlog_in.Count(x => x.Date > DateTime.Today),
                TotalPaid = 180,
                TotalUsers = userIdMapping.Count(),
                MaxPoints = gameovers.Max(x => x.score),
                TotalGameOversPerDay = gameovers.Count(x => x.Date > DateTime.Today),
                Payments = lastPayments
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
                    UserID = user.mapped_id,
                    PaymentStatus = "pending",
                    Date = DateTime.Now.AddHours(3)
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
                    protocol = new 
                    {
                        returnUrl = "http://patchipablo.ru/Home/Thanks",
                        callbackUrl = "http://patchipablo.ru:32789/api/MainApi/ProcessPayment"
                    },
                    paymentMethod = "bankcard"
                };

                string json = JsonConvert.SerializeObject(paymentRequest);

                _httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");

                var paymentUrl = @"https://paymaster.ru/api/v2/invoices";
                HttpResponseMessage response = await _httpClient.PostAsync(paymentUrl, new StringContent(json, Encoding.UTF8, "application/json"));

                string responseBody = await response.Content.ReadAsStringAsync();
                var responseObject = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(responseBody) ?? new Dictionary<string, string>();

                string paymentId = responseObject["paymentId"];
                payment.PaymentId = paymentId;
                _context.Payments.Add(payment);
                _context.SaveChanges();
                // Save();

                string url = responseObject["url"];

                return url;
            }
        }
        public async Task<Products> GetProducts(int id)
        {
            return await _context.Products.FirstAsync(x => x.Id == id);
        }
        public async Task UpdatePaymentStatus(string paymentId, string paymentStatus)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(x => x.PaymentId == paymentId);

            if (payment == null)
                throw new Exception("Not found payments");

            payment.PaymentStatus = paymentStatus;
            await _context.SaveChangesAsync();
        }
        public async Task<List<Products>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }
    }
}
