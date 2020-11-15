using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MII_Media.Models;
using MII_Media.Service;

namespace MII_Media.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService userService;
        private readonly IEmailService emailService;

        public HomeController(ILogger<HomeController> logger, IUserService userService, IEmailService emailService)
        {
            _logger = logger;
            this.userService = userService;
            this.emailService = emailService;
        }

        public IActionResult Index()
        {
            //var userId = userService.GetUserId();

            return View();
        }

        public IActionResult Privacy()
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
