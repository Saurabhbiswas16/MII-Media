using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MII_Media.Data;
using MII_Media.Models;
using MII_Media.Repository;

namespace MII_Media.Controllers
{
    [Authorize]
    public class FriendController : Controller
    {
        private readonly IFriendRepository friendRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly MiiContext miiContext;
        private readonly IPostRepository postRepository;

        public FriendController(IFriendRepository friendRepository, UserManager<ApplicationUser> userManager, MiiContext miiContext, IPostRepository postRepository)
        {
            this.friendRepository = friendRepository;
            this.userManager = userManager;
            this.miiContext = miiContext;
            this.postRepository = postRepository;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await userManager.GetUserAsync(User);
            return View();
        }

        //[HttpGet("send-request")]
        //public IActionResult SendRequest()
        //{

        //    return View();
        //}
        //[HttpPost("send-request")]
        //public async Task<IActionResult> SendRequest(Friend friend)
        //{
        //    var result = await friendRepository.SendRequestConfirmed(friend);
        //    return View(result);
        //}
        [HttpGet("send-request/${Email}")]
        public async Task<IActionResult> SendRequest(string Email)
        {
            var currentUser = await userManager.GetUserAsync(User);
            var result = await friendRepository.SendRequestConfirmed(currentUser.Email, Email);
            
            return RedirectToAction("ListUsers", "Friend");
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


            return RedirectToAction("GetAllFriends", "Friend");

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
            var currentUser = await userManager.GetUserAsync(User);
            var friend = await userManager.FindByEmailAsync(Email);
            ViewBag.friendProfile = friend;
            bool Confirmed = await friendRepository.FriendsConfirmed(currentUser.Email, Email);
            if (Confirmed)
            {
                ViewBag.AllPosts = postRepository.GetAllPosts(Email);
            }
            else
            {
                ViewBag.sendRequest = true;
            }
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
