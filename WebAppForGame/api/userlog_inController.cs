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

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(userlog_in, loadOptions));
        }
    }
}