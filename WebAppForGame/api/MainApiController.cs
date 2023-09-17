﻿using EFCoreDockerMySQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Web.Administration;
using Newtonsoft.Json;
using System.Text;
using WebAppForGame.Data;
using WebAppForGame.Dtos;
using WebAppForGame.Repository;

namespace WebAppForGame.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainApiController : ControllerBase
    {
        private readonly MainRepository _repository;
        private readonly ApplicationDbContext _context;
        public MainApiController(MainRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        [Route("log_login")]
        [HttpPost]
        public async Task<ActionResult> log_login(string userid)
        {
            try
            {
                var userlogin = new userlog_in()
                {
                    time = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
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
                    time = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
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
    }
}
