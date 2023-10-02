using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using EFCoreDockerMySQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace WebAppForGame.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class userlog_inController : Controller
    {
        private ApplicationDbContext _context;

        public userlog_inController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var userlog_in = _context.userlog_in.OrderByDescending(x => x.Date).Select(i => new
            {
                i.id,
                i.Date,
                i.user_id
            });

            return Json(await DataSourceLoader.LoadAsync(userlog_in, loadOptions));
        }
        [HttpDelete]
        public async Task Delete(Guid key)
        {
            var model = await _context.userlog_in.FirstOrDefaultAsync(item => item.id == key);

            _context.userlog_in.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}