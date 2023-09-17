using System.Collections;
using System.Globalization;
using DevExpress.Xpo;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using EFCoreDockerMySQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using WebAppForGame.Data;

namespace WebAppForGame.api
{
    [Route("api/[controller]/[action]")]
    public class PaymentsController : Controller
    {
        private ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var payments = _context.Payments.Select(i => new {
                i.Id,
                i.UserID,
                i.PaymentId,
                i.PaymentStatus,
                i.Date
            });

            return Json(await DataSourceLoader.LoadAsync(payments, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Payments();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Payments.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(Guid key, string values) {
            var model = await _context.Payments.FirstOrDefaultAsync(item => item.Id == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(Guid key) {
            var model = await _context.Payments.FirstOrDefaultAsync(item => item.Id == key);

            _context.Payments.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> ProductsLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Products
                         orderby i.Name
                         select new {
                             Value = i.Id,
                             Text = i.Name
                         };
            return Json(await DevExtreme.AspNet.Data.DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Payments model, IDictionary values) {
            string ID = nameof(Payments.Id);
            string USER_ID = nameof(Payments.UserID);
            string PAYMENT_ID = nameof(Payments.PaymentId);
            string PAYMENT_STATUS = nameof(Payments.PaymentStatus);
            string DATE = nameof(Payments.Date);

            if(values.Contains(ID)) {
                model.Id = ConvertTo<System.Guid>(values[ID]);
            }

            if(values.Contains(USER_ID)) {
                model.UserID = Convert.ToString(values[USER_ID]);
            }

            if(values.Contains(PAYMENT_ID)) {
                model.PaymentId = Convert.ToString(values[PAYMENT_ID]);
            }

            if(values.Contains(PAYMENT_STATUS)) {
                model.PaymentStatus = Convert.ToString(values[PAYMENT_STATUS]);
            }

            if(values.Contains(DATE)) {
                model.Date = Convert.ToDateTime(values[DATE]);
            }
        }

        private T ConvertTo<T>(object value) {
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
            if(converter != null) {
                return (T)converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
            } else {
                // If necessary, implement a type conversion here
                throw new NotImplementedException();
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}