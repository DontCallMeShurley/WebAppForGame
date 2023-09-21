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
                i.user_id,
                i.mapped_id
            });

            return Json(await DataSourceLoader.LoadAsync(userid_mapping, loadOptions));
        }

    }
}