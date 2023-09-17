using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppForGame.Repository;
using WebAppForGame.ViewModels;

namespace WebAppForGame.Controllers
{
    public class AdminController : Controller
    {
        private readonly MainRepository _repository;
        private readonly ProductsRepository _productsRepository;
        private readonly PaymentsRepository _paymentsRepository;
        

        public AdminController(MainRepository repository, ProductsRepository productsRepository, PaymentsRepository paymentsRepository)
        {
            _repository = repository;
            _productsRepository = productsRepository;
            _paymentsRepository = paymentsRepository;
        }

        // GET: AdminController
        public ActionResult Index()
        {
            var model = _repository.GetViewModel();
            return View(model);
        }

        // GET: AdminController/Payments
        public ActionResult Payments()
        {
            var model = _paymentsRepository.Get();
            return View(model);
        }

        // GET: AdminController/Products
        public ActionResult Products()
        {
            var model = _productsRepository.Get().Select(item => new ProductsViewModel()
            {
                Amount = item.Amount,
                Coins = item.Coins,
                Id = item.Id,
                Name = item.Name
            }).ToList();
            
            return View(model);
        }

        // GET: AdminController/Settings
        public ActionResult Settings()
        {
            // var model = _repository.GetSettingsViewModel();
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}