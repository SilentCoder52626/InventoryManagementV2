using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using System.Diagnostics;
using InventoryLibrary.Source.Services.ServiceInterface;

namespace WebApp.Controllers
{
    

    public class HomeController : Controller
    {
        private readonly CustomerTransactionServiceInterface _customerTransactionService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, CustomerTransactionServiceInterface customerTransactionService)
        {
            _logger = logger;
            _customerTransactionService = customerTransactionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ReCalculateCustomerBalance()
        {

            return RedirectToAction(nameof(Index));
        }

    }
}