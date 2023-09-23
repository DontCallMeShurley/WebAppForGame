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
        private readonly ILogger _loggerTest;
        public MainApiController(MainRepository repository,  ILogger<MainApiController> logger)
        {
            _repository = repository;
            _loggerTest = logger;
        }

        [Route("log_login")]
        [HttpPost]
        public async Task<ActionResult> log_login(string userid)
        {
            try
            {
                await _repository.log_login(userid);
            }
            catch (Exception e)
            {
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
                await _repository.log_gameover(log_Gameover);
            }
            catch (Exception e)
            {
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
                var result = await _repository.GetMappedUserId(userid);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [Route("GetSerialNumber")]
        [HttpGet]
        public async Task<ActionResult> GetSerialNumber(string userid)
        {
            try
            {
                var result = await _repository.GetSerialNumber(userid);
                return Ok(result);
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
                var result = await _repository.GetIDWithSN(userid);
                return Ok(result);
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
                var result = await _repository.CreateUserById(userid);
                return Ok(result);
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
                var id = test.Id ?? 0;
                var status = test.status;

                _loggerTest.LogInformation($"Proccess payment {test.Id}");

                if (id != 0 && status != null)
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

        [Route("Log_GameStart")]
        [HttpPost]
        public async Task<IActionResult> Log_GameStart(string userId)
        {
            try
            {
                await _repository.Log_GameStart(userId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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
