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
    public class ProductsApiController : Controller
    {
        private ApplicationDbContext _context;

        public ProductsApiController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var products = _context.Products.Select(i => new {
                i.Id,
                i.Amount,
                i.Name,
                i.Coins,
                i.Description
            });

            return Json(await DataSourceLoader.LoadAsync(products, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Products();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Products.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Products.FirstOrDefaultAsync(item => item.Id == key);
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
        public async Task Delete(int key) {
            var model = await _context.Products.FirstOrDefaultAsync(item => item.Id == key);
            var allPayments = await _context.Payments.Include(x => x.Product).Where(x => x.Product == model).ToListAsync();
            allPayments.ForEach(x => x.Product = null);

            _context.Products.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Products model, IDictionary values) {
            string ID = nameof(Products.Id);
            string AMOUNT = nameof(Products.Amount);
            string NAME = nameof(Products.Name);
            string COINS = nameof(Products.Coins);
            string DESCRIPTION = nameof(Products.Description);

            if(values.Contains(ID)) {
                model.Id = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(AMOUNT)) {
                model.Amount = Convert.ToDouble(values[AMOUNT], CultureInfo.InvariantCulture);
            }

            if(values.Contains(NAME)) {
                model.Name = Convert.ToString(values[NAME]);
            }

            if(values.Contains(COINS)) {
                model.Coins = Convert.ToInt32(values[COINS]);
            }

            if(values.Contains(DESCRIPTION)) {
                model.Description = Convert.ToString(values[DESCRIPTION]);
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