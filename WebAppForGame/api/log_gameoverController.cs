using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreDockerMySQL;
using Microsoft.AspNetCore.Authorization;

namespace WebAppForGame.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class log_gameoverController : Controller
    {
        private ApplicationDbContext _context;

        public log_gameoverController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var log_gameover = _context.log_gameover.OrderByDescending(x => x.Date).Select(i => new
            {
                i.id,
                i.Date,
                i.user_id,
                i.score
            });

            return Json(await DataSourceLoader.LoadAsync(log_gameover, loadOptions));
        }
        [HttpDelete]
        public async Task Delete(Guid key)
        {
            var model = await _context.log_gameover.FirstOrDefaultAsync(item => item.id == key);

            _context.log_gameover.Remove(model);
            await _context.SaveChangesAsync();
        }

    }
}