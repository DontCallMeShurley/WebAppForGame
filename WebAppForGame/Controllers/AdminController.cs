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
        private readonly PaymentsRepository _paymentsRepository;


        public AdminController(MainRepository repository, PaymentsRepository paymentsRepository)
        {
            _repository = repository;
            _paymentsRepository = paymentsRepository;
        }

        // GET: AdminController
        public ActionResult Index()
        {
            var model = _repository.GetViewModel();
            return View(model);
        }

        public ActionResult Products()
        {
            return View();
        }
        public ActionResult Payments()
        {
            return View();
        }
        // GET: AdminController/Settings
        public ActionResult Settings()
        {
            // var model = _repository.GetSettingsViewModel();
            return View();
        }
    }
}