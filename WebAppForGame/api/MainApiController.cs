using EFCoreDockerMySQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Web.Administration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System.Text;
using WebAppForGame.Controllers;
using WebAppForGame.Data;
using WebAppForGame.Dtos;
using WebAppForGame.Repository;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace WebAppForGame.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainApiController : ControllerBase
    {
        private readonly MainRepository _repository;
        private readonly ApplicationDbContext _context;
        private readonly ILogger _loggerTest;
        public MainApiController(MainRepository repository, ApplicationDbContext context, ILogger<MainApiController> logger)
        {
            _repository = repository;
            _context = context;
            _loggerTest = logger;
        }
        [Route("log_login")]
        [HttpPost]
        public async Task<ActionResult> log_login(string userid)
        {
            try
            {
                var userlogin = new userlog_in()
                {
                    Date = DateTime.Now.AddHours(3),
                    user_id = userid
                };
                _context.userlog_in.Add(userlogin);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
            return Ok("Successfull");
        }

        [Route("log_gameover")]
        [HttpPost]
        public async Task<ActionResult> log_gameover(log_gameover_dto log_Gameover)
        {
            try
            {
                var gameover_log = new log_gameover()
                {
                    Date = DateTime.Now.AddHours(3),
                    user_id = log_Gameover.user_id,
                    score = log_Gameover.score
                };
                _context.log_gameover.Add(gameover_log);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
            return Ok("Successful");
        }

        [Route("GetMappedUserId")]
        [HttpGet]
        public async Task<ActionResult> GetMappedUserId(string userid)
        {
            try
            {
                var count = await _context.userid_mapping.CountAsync(x => x.user_id == userid);
                if (count == 0)
                {
                    var mappedId = getUniqueId(7);

                    _context.userid_mapping.Add(new userid_mapping
                    {
                        user_id = userid,
                        mapped_id = mappedId
                    });
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
            return Ok(_context.userid_mapping.FirstOrDefault(x => x.user_id == userid)?.mapped_id);
        }

        [Route("GetSerialNumber")]
        [HttpGet]
        public async Task<ActionResult> GetSerialNumber(string userid)
        {
            try
            {
                var sn = await _context.SerialNumbers.FirstOrDefaultAsync(x => x.user_id == userid);
                if (sn == null)
                {
                    var mappedId = getUniqueId(10, true);

                    sn = new SerialNumbers
                    {
                        user_id = userid,
                        serial_number = mappedId
                    };
                    _context.SerialNumbers.Add(sn);
                    _context.SaveChanges();
                }
                return Ok(sn.serial_number);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }
        [Route("GetIDWithSN")]
        [HttpGet]
        public async Task<ActionResult> GetIDWithSN(string userid)
        {
            try
            {
                string serialNumber = await getOrSetSerialNumber(userid);
                string mappedId = await getOrSetMappedId(userid);


                var json = new { serial_number = serialNumber, mappedId = mappedId };

                var jsonResult = JsonConvert.SerializeObject(json);

                return Ok(jsonResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }
        [Route("CreateUserById")]
        [HttpPost]
        public async Task<ActionResult> CreateUserById(string userid)
        {
            try
            {
                var count = await _context.userid_mapping.CountAsync(x => x.user_id == userid);
                if (count > 0)
                    return BadRequest("Ошибка. Для пользователя уже заведён смапенный аккаунт");

                var mappedId = getUniqueId(7);

                _context.userid_mapping.Add(new userid_mapping
                {
                    user_id = userid,
                    mapped_id = mappedId
                });
                await _context.SaveChangesAsync();
                return Ok(mappedId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [Route("CreatePayment")]
        [HttpGet]
        public async Task<ActionResult> CreatePayment(string userID, int productID)
        {
            try
            {
                var result = await _repository.GetPayLink(userID, productID);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }
        [Route("ProcessPayment")]
        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PayMasterData test)
        {
            try
            {
                var id = test.Id;
                var status = test.status;

                _loggerTest.LogInformation($"Proccess payment {test.Id}");


                if (id != null && status != null)
                    await _repository.UpdatePaymentStatus(id.ToString(), status);
                else
                    return BadRequest("Ошибка в вводимых данных");
                return Ok();
            }
            catch (Exception e)
            {
                _loggerTest.LogError($"Proccess payment failed. {e.Message}");
                return BadRequest(e.Message);
            }
        }

        private async Task<string> getOrSetSerialNumber(string userId)
        {
            var sn = await _context.SerialNumbers.FirstOrDefaultAsync(x => x.user_id == userId);
            if (sn == null)
            {

                sn = new SerialNumbers
                {
                    user_id = userId,
                    serial_number = getUniqueId(10, true)
                };
                await _context.SerialNumbers.AddAsync(sn);
                await _context.SaveChangesAsync();
            }
            return sn.serial_number;
        }
        private async Task<string> getOrSetMappedId(string userId)
        {
            var sn = await _context.userid_mapping.FirstOrDefaultAsync(x => x.user_id == userId);
            if (sn == null)
            {

                sn = new userid_mapping
                {
                    user_id = userId,
                    mapped_id = getUniqueId(7)
                };
                await _context.userid_mapping.AddAsync(sn);
                await _context.SaveChangesAsync();
            }
            return sn.mapped_id;
        }
        private string getUniqueId(int maxLength, bool isSerial = false)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
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
                        if (db.SerialNumbers.Any(x => x.serial_number == output.ToString()))
                            result = getUniqueId(maxLength, true);
                    }
                    else
                    {
                        if (db.userid_mapping.Any(x => x.mapped_id == output.ToString()))
                            result = getUniqueId(maxLength);
                    }


                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return "";
                }
            }
        }
        #region jsonFromPayMaster
        public class Amount
        {
            public decimal Value { get; set; }
            public string? Currency { get; set; }
        }

        public class Params
        {
            [JsonProperty("BT_USR")]
            public string? BT_USR { get; set; }
        }

        public class Invoice
        {
            public string? description { get; set; }
            public Params? Params { get; set; }
        }

        public class PaymentData
        {
            [JsonProperty("paymentMethod")]
            public string? paymentMethod { get; set; }

            [JsonProperty("paymentInstrumentTitle")]
            public string? paymentInstrumentTitle { get; set; }
        }

        public class PayMasterData
        {
            public int? Id { get; set; }
            public DateTime created { get; set; }
            public bool testMode { get; set; }
            public string? status { get; set; }
            public string? merchantId { get; set; }
            public Amount? amount { get; set; }
            public Invoice? invoice { get; set; }
            public PaymentData? paymentData { get; set; }
        }
        #endregion
    }
}
