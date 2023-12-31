﻿using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using WebAppForGame.Models;
using WebAppForGame.Repository;

namespace WebAppForGame.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MainRepository _repository;

        public HomeController(ILogger<HomeController> logger, MainRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }
      
        public async Task<IActionResult> Store(string userId)
        {
            var check = await _repository.CheckUserID(userId);

            if (userId == null || !check)
                return NotFound();

            var model = await _repository.GetProducts();

            ViewBag.UserId = userId;
            return View(model);
        }

        public IActionResult HowToGet()
        {
            return View();
        }
        public FileResult Download()
        {
            string fileName = "SpeedBox.apk";

            string fileType = "application/vnd.android.package-archive";

            return File("/files/SpeedBox.apk", fileType, fileName);
        }
        public IActionResult Thanks()
        {
            return View();
        }
        public IActionResult Contacts()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}