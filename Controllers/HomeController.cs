using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MII_Media.Data;
using MII_Media.Models;
using MII_Media.Service;

namespace MII_Media.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService userService;
        private readonly IEmailService emailService;
        private readonly MiiContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(ILogger<HomeController> logger, IUserService userService, IEmailService emailService, MiiContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            this.userService = userService;
            this.emailService = emailService;
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            //var userId = userService.GetUserId();
            string email = await userManager.GetEmailAsync(await userManager.GetUserAsync(User));
            IList<string> friendLists= context.Friends.Where(c => c.User1 == email && c.Confirmed==true).Select(c => c.User2).ToList();
            friendLists.Add(email);
            IList<Post> displayPostList = new List<Post>();
            foreach(string mail in friendLists)
            {
                var postLists = context.Posts.Where(p => p.AppUser == mail).ToList();
                foreach(var l in postLists)
                {
                    displayPostList.Add(l);
                }
            }
            displayPostList.OrderByDescending(p => p.PostId);
            return View(displayPostList);
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
