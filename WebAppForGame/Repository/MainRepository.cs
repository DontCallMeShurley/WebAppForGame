using EFCoreDockerMySQL;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using WebAppForGame.ViewModels;
using System.Configuration;
using System.Net.Http;
using NLog;
using WebAppForGame.Enums;
using WebAppForGame.Dtos;
using WebAppForGame.Data;

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
                TotalPaid = _context.Payments.Where(x => x.PaymentStatus == StatusPayment.Settled).Sum(x => x.Product != null ? x.Product.Amount : 0),
                TotalUsers = userIdMapping.Count(),
                MaxPoints = gameovers != null && gameovers.Any() ? gameovers.Max(x => x.score) : 0,
                TotalGameOversPerDay = gameovers != null && gameovers.Any() ?  gameovers.Count(x => x.Date > DateTime.Today) : 0,
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
                    PaymentStatus = StatusPayment.Pending,
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
                        returnUrl = "speedbox://mylink?Menu1",
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

        public async Task<int> Log_GameStart(string userId)
        {
            await checkUserID(userId);

            await _context.Log_GameStart.AddAsync(new Log_GameStart()
            {
                Date = DateTime.Now,
                UserID = userId
            });
            await _context.SaveChangesAsync();

            return await getAvaliableCoins(userId);
        }

        public async Task<Products> GetProduct(int id)
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

        public async Task log_login(string userId)
        {
            await checkUserID(userId);

            var userlogin = new userlog_in()
            {
                Date = DateTime.Now.AddHours(3),
                user_id = userId
            };
            _context.userlog_in.Add(userlogin);
            await _context.SaveChangesAsync();
        }

        public async Task log_gameover(log_gameover_dto log_Gameover)
        {
            await checkUserID(log_Gameover.user_id);

            var gameover_log = new log_gameover()
            {
                Date = DateTime.Now.AddHours(3),
                user_id = log_Gameover.user_id,
                score = log_Gameover.score
            };
            _context.log_gameover.Add(gameover_log);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetMappedUserId(string userId)
        {
            var count = await _context.userid_mapping.CountAsync(x => x.user_id == userId);
            if (count == 0)
            {
                var mappedId = await getUniqueId(7);

                await _context.userid_mapping.AddAsync(new userid_mapping
                {
                    user_id = userId,
                    mapped_id = mappedId
                });
                await _context.SaveChangesAsync();
            }

            var result = await _context.userid_mapping.FirstOrDefaultAsync(x => x.user_id == userId);

            if (result == null)
                throw new Exception("Some problems. Not found mappedId");

            return result.mapped_id ?? "";
        }

        public async Task<string> GetSerialNumber(string userid)
        {
            var sn = await _context.SerialNumbers.FirstOrDefaultAsync(x => x.user_id == userid);
            if (sn == null)
            {
                var mappedId = getUniqueId(10, true);

                sn = new SerialNumbers
                {
                    user_id = userid,
                    serial_number = await mappedId
                };
                await _context.SerialNumbers.AddAsync(sn);
                await _context.SaveChangesAsync();
            }
            return sn.serial_number ?? "";
        }

        public async Task<string> GetIDWithSN(string userid)
        {
            string serialNumber = await getOrSetSerialNumber(userid);
            string mappedId = await getOrSetMappedId(userid);

            var avaliableCoins = await getAvaliableCoins(mappedId);

            var number = await _context.UserNumbers.FirstOrDefaultAsync(x => x.user_id == userid) ?? new UserNumber();

            var json = new { serial_number = serialNumber, mappedId, avaliableCoins, number.number };

            var userlogin = new userlog_in()
            {
                Date = DateTime.Now.AddHours(3),
                user_id = mappedId
            };
            _context.userlog_in.Add(userlogin);
            await _context.SaveChangesAsync();

            var jsonResult = JsonConvert.SerializeObject(json);

            return jsonResult;
        }

        public async Task<string> CreateUserById(string userid)
        {
            var count = await _context.userid_mapping.CountAsync(x => x.user_id == userid);
            if (count > 0)
                throw new Exception("Ошибка. Для пользователя уже заведён смапенный аккаунт");

            var mappedId = await getUniqueId(7);

            _context.userid_mapping.Add(new userid_mapping
            {
                user_id = userid,
                mapped_id = mappedId
            });
            await _context.SaveChangesAsync();
            return mappedId;
        }

        public async Task<bool> CheckUserID(string userId)
        {
            var isExist = await _context.userid_mapping.AnyAsync(x => x.mapped_id == userId);
            return isExist;
        }
        public async Task CreateUserNumber(string userId, int number)
        {
            await _context.UserNumbers.AddAsync(new UserNumber
            {
                user_id = userId,
                number = number
            });
            await _context.SaveChangesAsync();
            return;
        }

        private async Task checkUserID(string userID)
        {
            var user = await _context.userid_mapping.FirstOrDefaultAsync(x => x.mapped_id == userID);

            if (user == null)
                throw new Exception("Not found user");
        }

        private async Task<string> getOrSetSerialNumber(string userId)
        {
            var sn = await _context.SerialNumbers.FirstOrDefaultAsync(x => x.user_id == userId);
            if (sn == null)
            {
                sn = new SerialNumbers
                {
                    user_id = userId,
                    serial_number = await getUniqueId(10, true)
                };
                await _context.SerialNumbers.AddAsync(sn);
                await _context.SaveChangesAsync();
            }
            return sn.serial_number ?? "";
        }

        private async Task<string> getOrSetMappedId(string userId)
        {
            var sn = await _context.userid_mapping.FirstOrDefaultAsync(x => x.user_id == userId);
            if (sn == null)
            {

                sn = new userid_mapping
                {
                    user_id = userId,
                    mapped_id = await getUniqueId(7)
                };
                await _context.userid_mapping.AddAsync(sn);
                await _context.SaveChangesAsync();
            }
            return sn.mapped_id ?? "";
        }

        private async Task<string> getUniqueId(int maxLength, bool isSerial = false)
        {

            try
            {
                string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                StringBuilder output = new StringBuilder();
                Random rnd = new Random();
                string result = "";

                for (int i = 0; i < maxLength; i++)
                {
                    var index = rnd.Next(characters.Length);
                    output.Append(characters[index]);
                }
                result = output.ToString();

                if (isSerial)
                {
                    if (await _context.SerialNumbers.AnyAsync(x => x.serial_number == output.ToString()))
                        result = await getUniqueId(maxLength, true);
                }
                else
                {
                    if (await _context.userid_mapping.AnyAsync(x => x.mapped_id == output.ToString()))
                        result = await getUniqueId(maxLength);
                }


                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }

        }

        private async Task<int> getAvaliableCoins(string mappedId)
        {
            var paymentsByUser = await _context.Payments.Include(x => x.Product).Where(x => x.UserID == mappedId && x.PaymentStatus == StatusPayment.Settled).ToListAsync();

            var avaliableCoins =  paymentsByUser.Sum(x => x.Product.Coins)
                                - await _context.Log_GameStart.CountAsync(x => x.UserID == mappedId);

            if (avaliableCoins > 0)
            {
                var today = DateTime.Today;
                avaliableCoins = avaliableCoins - paymentsByUser.Where(x => (today - x.Date).Days >= 7).Sum(x => x.Product.Coins);
            }
            else
                avaliableCoins = 0;

            return avaliableCoins;
        }
    }
}
