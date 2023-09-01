using EFCoreDockerMySQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebAppForGame.Data;

namespace WebAppForGame.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainApiController : ControllerBase
    {

        [Route("log_login")]
        [HttpPost]
        public async Task<ActionResult> log_login([FromBody] string userid)
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
                    return BadRequest(e.Message);
                }
            }
            return Ok();
        }

        [Route("log_gameover")]
        [HttpPost]
        public async Task<ActionResult> log_gameover([FromBody] string userid, [FromBody] string score)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    var gameover_log = new log_gameover()
                    {
                        time = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                        user_id = userid,
                        score = long.Parse(score)
                    };
                    db.log_gameover.Add(gameover_log);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return Ok();
        }
        [Route("GetMappedUserId")]
        [HttpPost]
        public async Task<ActionResult> GetMappedUserId([FromBody] string userid)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var count = db.userid_mapping.Count(x => x.user_id == userid);
                if (count == 0)
                    return BadRequest("WTF dude, нет айди под этот айди");

                return Ok(db.userid_mapping.FirstOrDefault(x => x.user_id == userid)?.mapped_id);
            }
        }
    }
}
