using Microsoft.AspNetCore.Mvc;
using WebAppForGame.Repository;

namespace WebAppForGame.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MainRepository _repository;

        public ProductsController(ILogger<HomeController> logger, MainRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task<IActionResult> Index(string userId)
        {
            var check = await _repository.CheckUserID(userId);

            if (userId == null || !check)
                return NotFound();

            var model = await _repository.GetProducts();

            ViewBag.UserId = userId;
            return View(model);
        }
    }
}
