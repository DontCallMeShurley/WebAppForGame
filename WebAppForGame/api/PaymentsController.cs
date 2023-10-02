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

        [HttpDelete]
        public async Task Delete(Guid key)
        {
            var model = await _context.Payments.FirstOrDefaultAsync(item => item.Id == key);

            _context.Payments.Remove(model);
            await _context.SaveChangesAsync();
        }

    }
}