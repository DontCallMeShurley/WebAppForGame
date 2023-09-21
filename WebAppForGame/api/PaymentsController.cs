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
    public class PaymentsController : Controller
    {
        private ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var payments = _context.Payments.Include(x => x.Product).OrderByDescending(x => x.Date).Select(i => new
            {
                i.Id,
                i.UserID,
                i.PaymentId,
                i.PaymentStatus,
                i.Date,
                i.Product
            });

            return Json(await DataSourceLoader.LoadAsync(payments, loadOptions));
        }

    }
}