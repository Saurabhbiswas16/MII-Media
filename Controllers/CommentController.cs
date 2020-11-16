using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using MII_Media.Models;
using MII_Media.Repository;
using MII_Media.ViewModels;

namespace MII_Media.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _comRepo;

        public CommentController(ICommentRepository comRepo)
        {
            _comRepo = comRepo;
        }
        [HttpGet]
        public IActionResult Create(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CommentCreateViewModel _comment)
        {
            Comment comment = null;
            if (ModelState.IsValid)
            {
                comment = new Comment
                {
                    Message = _comment.Message,
                    CommentTime = DateTime.Now,
                    PostId = _comment.PostId
                };
                _comRepo.Add(comment);
                return RedirectToAction("Details", "Post", new { id = comment.PostId });
            }
            return View(comment);
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
            return RedirectToAction("Index", "Post");
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
