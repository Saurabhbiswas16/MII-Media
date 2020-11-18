using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MII_Media.Models;
using MII_Media.Repository;
using MII_Media.ViewModels;

namespace MII_Media.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepo;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly UserManager<ApplicationUser> userManager;

        private string curUser;

        
        public PostController(IPostRepository postRepo, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _postRepo = postRepo;
            this.hostingEnvironment = hostingEnvironment;
            this.userManager = userManager;
        }

        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PostCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Post != null)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Post.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Post.CopyTo(fileStream);
                    }
                }
                curUser = await userManager.GetEmailAsync(await userManager.GetUserAsync(User));
                Post post = new Post
                {
                    PostPath = uniqueFileName,
                    Caption = model.Caption,
                    AppUser=curUser
                };
                var wait = (model.UploadTime - DateTime.Now).TotalMilliseconds;
                if (wait >= 0)
                {
                    BackgroundJob.Schedule(() => UploadPhoto(post), TimeSpan.FromMilliseconds(wait));
                }

                
                return RedirectToAction("index");
            }
            return View();
        }
        public async Task<ViewResult> Index()
        {
            var model = _postRepo.GetAllPosts(await userManager.GetEmailAsync(await userManager.GetUserAsync(User)));
            return View(model);
        }
        public ViewResult Details(int Id)
        {
            Post post = _postRepo.GetPost(Id);
            if (post == null)
            {
                Response.StatusCode = 404;
                return View("PostNotFound", Id);
            }
            return View(post);
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Post post = _postRepo.GetPost(Id);
            if (post == null)
            {
                Response.StatusCode = 404;
                return View("PostNotFound", Id);
            }
            return View(post);
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeleteConfirmed(int Id)
        {
            var post = _postRepo.GetPost(Id);
            _postRepo.Delete(post.PostId);
            return RedirectToAction("index");
        }
        [HttpGet]
        public ViewResult Edit(int Id)
        {
            Post post = _postRepo.GetPost(Id);
            var newPost = new PostEditViewModel
            {
                Id = post.PostId,
                Caption = post.Caption
            };
            return View(newPost);
        }
        [HttpPost]
        public IActionResult Edit(PostEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Post post = _postRepo.GetPost(model.Id);
                post.Caption = model.Caption;
                Post updatedPost = _postRepo.Update(post);
                return RedirectToAction("details", new { id = post.PostId });
            }
            return View(model);
        }

        public void UploadPhoto(Post post)
        {
            _postRepo.Add(post);
        }
    }
}
