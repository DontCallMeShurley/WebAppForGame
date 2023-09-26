using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections;
using EFCoreDockerMySQL;
using WebAppForGame.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebAppForGame.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class SettingsController : Controller
    {
        private ApplicationDbContext _context;

        public SettingsController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var settings = _context.Settings.Select(i => new {
                i.Id,
                i.BearerToken,
                i.MerchantId
            });

            return Json(await DataSourceLoader.LoadAsync(settings, loadOptions));
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Settings.FirstOrDefaultAsync(item => item.Id == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }


        private void PopulateModel(Settings model, IDictionary values) {
            string ID = nameof(Settings.Id);
            string BEARER_TOKEN = nameof(Settings.BearerToken);
            string MERCHANT_ID = nameof(Settings.MerchantId);

            if(values.Contains(ID)) {
                model.Id = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(BEARER_TOKEN)) {
                model.BearerToken = Convert.ToString(values[BEARER_TOKEN]);
            }

            if(values.Contains(MERCHANT_ID)) {
                model.MerchantId = Convert.ToString(values[MERCHANT_ID]);
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