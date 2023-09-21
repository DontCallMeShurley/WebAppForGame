using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using EFCoreDockerMySQL;
using WebAppForGame.Data;

namespace WebAppForGame.Controllers
{
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

    }
}