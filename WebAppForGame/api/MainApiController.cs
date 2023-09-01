using EFCoreDockerMySQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using WebAppForGame.Data;
using WebAppForGame.Dtos;

namespace WebAppForGame.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainApiController : ControllerBase
    {

        [Route("log_login")]
        [HttpPost]
        public async Task<ActionResult> log_login(string userid)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    var userlogin = new userlog_in()
                    {
                        time = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                        user_id = userid
                    };
                    db.userlog_in.Add(userlogin);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Ok("Successfull");
        }

        [Route("log_gameover")]
        [HttpPost]
        public async Task<ActionResult> log_gameover(log_gameoverDto log_Gameover)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            { 
                try
                {
                    var gameover_log = new log_gameover()
                    {
                        time = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                        user_id = log_Gameover.user_id,
                        score = log_Gameover.score
                    };
                    db.log_gameover.Add(gameover_log);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
            return Ok("Successfull");
        }

        [Route("GetMappedUserId")]
        [HttpPost]
        public async Task<ActionResult> GetMappedUserId(string userid)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    var count = await db.userid_mapping.CountAsync(x => x.user_id == userid);
                    if (count == 0)
                    {
                        var mappedId = getUniqueId(4);

                        db.userid_mapping.Add(new userid_mapping
                        {
                            user_id = userid,
                            mapped_id = mappedId
                        });
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
                return Ok(db.userid_mapping.FirstOrDefault(x => x.user_id == userid)?.mapped_id);
            }
        }

        [Route("CreateUserById")]
        [HttpPost]
        public async Task<ActionResult> CreateUserById(string userid)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    var count = await db.userid_mapping.CountAsync(x => x.user_id == userid);
                    if (count > 0)
                        return BadRequest("Ошибка. Для пользователя уже заведён смапенный аккаунт");

                    var mappedId = getUniqueId(4);

                    db.userid_mapping.Add(new userid_mapping
                    {
                        user_id = userid,
                        mapped_id = mappedId
                    });
                    await db.SaveChangesAsync();
                    return Ok(mappedId);
                }
                catch  (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest(e.Message);
                }
            }
        }

        private string getUniqueId(int maxLength)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    string characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    StringBuilder output = new StringBuilder();
                    Random rnd = new Random();
                    string result = "";

                    for (int i = 0; i < maxLength; i++)
                    {

                        var index = rnd.Next(characters.Length);
                        output.Append(characters[index]);
                    }
                    result = output.ToString();

                    if (db.userid_mapping.Any(x => x.mapped_id == output.ToString()))
                        result = getUniqueId(maxLength);

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
