using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using MII_Media.Models;
using MII_Media.Repository;
using MII_Media.ViewModels;

namespace MII_Media.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _comRepo;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentController(ICommentRepository comRepo, UserManager<ApplicationUser> userManager)
        {
            _comRepo = comRepo;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Create(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentCreateViewModel _comment)
        {
            Comment comment = null;
            if (ModelState.IsValid)
            {
                comment = new Comment
                {
                    Message = _comment.Message,
                    CommentTime = DateTime.Now,
                    PostId = _comment.PostId,
                    Commenter = await userManager.GetEmailAsync(await userManager.GetUserAsync(User))
                };
                _comRepo.Add(comment);
                return RedirectToAction("Details", "Post", new { id = comment.PostId });
            }
            return View(comment);
        }
        [HttpPost]
        public async Task<IActionResult> CreateComment(string msg,int id)
        {
            Comment comment = null;
            if (msg != null)
            {
                comment = new Comment
                {
                    Message = msg,
                    CommentTime = DateTime.Now,
                    PostId = id,
                    Commenter = await userManager.GetEmailAsync(await userManager.GetUserAsync(User))
                };
                _comRepo.Add(comment);
                return RedirectToAction("Details", "Post", new { id = comment.PostId });
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Comment comment = _comRepo.GetComment(Id);
            if (comment == null)
            {
                Response.StatusCode = 404;
                return View("CommentNotFound", Id);
            }
            //var com = new CommentCreateViewModel
            //{
            //    PostId = comment.PostId,
            //    Message = comment.Message
            //};
            return View(comment);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeleteConfirmed(int Id)
        {
            var comment = _comRepo.GetComment(Id);
            _comRepo.Delete(comment.CommentId);
            return RedirectToAction("Profile", "Account");
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
