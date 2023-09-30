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
using Microsoft.AspNetCore.Authorization;

namespace WebAppForGame.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class UserNumbersController : Controller
    {
        private ApplicationDbContext _context;

        public UserNumbersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {


            var usernumbers = _context.UserNumbers.Select(i => new
            {
                i.id,
                user_id = _context.userid_mapping.First(x => x.user_id == i.user_id).mapped_id ?? i.user_id,
                i.number
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(usernumbers, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            var model = new UserNumber();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.UserNumbers.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(Guid key, string values)
        {
            var model = await _context.UserNumbers.FirstOrDefaultAsync(item => item.id == key);
            if (model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(Guid key)
        {
            var model = await _context.UserNumbers.FirstOrDefaultAsync(item => item.id == key);

            _context.UserNumbers.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(UserNumber model, IDictionary values)
        {
            string ID = nameof(UserNumber.id);
            string USER_ID = nameof(UserNumber.user_id);
            string NUMBER = nameof(UserNumber.number);

            if (values.Contains(ID))
            {
                model.id = ConvertTo<System.Guid>(values[ID]);
            }

            if (values.Contains(USER_ID))
            {
                model.user_id = Convert.ToString(values[USER_ID]);
            }

            if (values.Contains(NUMBER))
            {
                model.number = values[NUMBER] != null ? Convert.ToInt32(values[NUMBER]) : (int?)null;
            }
        }

        private T ConvertTo<T>(object value)
        {
            var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
            {
                return (T)converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
            }
            else
            {
                // If necessary, implement a type conversion here
                throw new NotImplementedException();
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}