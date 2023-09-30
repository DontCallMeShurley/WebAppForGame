using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using EFCoreDockerMySQL;
using Microsoft.AspNetCore.Authorization;

namespace WebAppForGame.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class userid_mappingController : Controller
    {
        private ApplicationDbContext _context;

        public userid_mappingController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var userid_mapping = _context.userid_mapping.Select(i => new {
                i.id,
                userId = i.user_id,
                mappedId = i.mapped_id,
                serialNumber = _context.SerialNumbers.First(x=>x.user_id == i.user_id).serial_number,
                _context.UserNumbers.First(x => x.user_id == i.user_id).number

            });

            return Json(await DataSourceLoader.LoadAsync(userid_mapping, loadOptions));
        }

    }
}