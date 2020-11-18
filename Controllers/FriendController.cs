using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MII_Media.Data;
using MII_Media.Models;
using MII_Media.Repository;

namespace MII_Media.Controllers
{
    public class FriendController : Controller
    {
        private readonly IFriendRepository friendRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly MiiContext miiContext;

        public FriendController(IFriendRepository friendRepository, UserManager<ApplicationUser> userManager, MiiContext miiContext)
        {
            this.friendRepository = friendRepository;
            this.userManager = userManager;
            this.miiContext = miiContext;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await userManager.GetUserAsync(User);
            return View();
        }

        [HttpGet("send-request")]
        public IActionResult SendRequest()
        {

            return View();
        }
        [HttpPost("send-request")]
        public async Task<IActionResult> SendRequest(Friend friend)
        {
            var result = await friendRepository.SendRequestConfirmed(friend);
            return View(result);
        }

        [HttpGet("ReceiveRequest")]
        public async Task<IActionResult> ReceiveRequest()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var result = await friendRepository.GetAllReceiveRequest(currentUser.Email);
            return View(result);
        }

        [HttpGet("ReceiveRequestConfirmed/{id}")]
        public async Task<IActionResult> ReceiveRequestConfirmed(int id)
        {

            var currentUser = await userManager.GetUserAsync(User);

            var result = await friendRepository.ConfirmedRequestReceive(id);


            return View();

        }

        [HttpGet("GetAllFriends")]
        public async Task<IActionResult> GetAllFriends()
        {
            var currentUser = await userManager.GetUserAsync(User);

            var result = await friendRepository.FetchedAllFriends(currentUser.Email);
            return View(result);
        }
        [HttpGet("FriendsProfile/${Email}")]
        public async Task<IActionResult> FriendsProfile(string Email)
        {
            var User = await userManager.FindByEmailAsync(Email);
            ViewBag.friendProfile = User;
            /*if ()
            {
                
            }
            else
            {

            }*/
            // var result = await friendRepository.FetchedAllFriends(currentUser.Email);
            return View();
        }
        [HttpGet("ListUsers")]
        public ActionResult ListUsers()
        {
            return View(miiContext.Users.ToList());
        }


        [HttpGet("UnknownFriendsProfile/${Email}")]
        public async Task<IActionResult> UnknownFriendsProfile(string Email)
        {
            //var currentUser = await userManager.GetUserAsync(User);

            var result = await friendRepository.FetchedAllFriends(Email);
            return View(result);
        }
    }
}
