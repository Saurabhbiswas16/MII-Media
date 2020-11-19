using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MII_Media.Data;
using MII_Media.Models;
using MII_Media.Repository;
using MII_Media.ViewModels;

namespace MII_Media.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountRepository accountRepository;
        private readonly IPostRepository postRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(IAccountRepository accountRepository, IPostRepository postRepository, UserManager<ApplicationUser> userManager)
        {
            this.accountRepository = accountRepository;
            this.postRepository = postRepository;
            this.userManager = userManager;
        }

        // private MiiContext context = new MiiContext();
        //public AccountController(IAccountRepository accountRepository)
        //{
        //    this.accountRepository = accountRepository;
        //}
        [AllowAnonymous]
        [Route("signup")]
        public IActionResult Signup()
        {
            return View();
        }
        [AllowAnonymous]
        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> Signup(SignUpUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                string OTP = RandomString(6);
                var result = await accountRepository.CreateUserAsync(userModel,OTP);
                if (!result.Succeeded)
                {
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    return View(userModel);
                }
                ModelState.Clear();
                // return RedirectToAction("ConfirmEmail", new { email = userModel.Email });
                return RedirectToAction("emailConfirmed");
            }
            return View(userModel);
        }
        [AllowAnonymous]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(SignInModel signInModel)
        {
            var result = await accountRepository.PasswordSignInAsync(signInModel);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError("", "Not allowed to login");
            }
            else
            {
                ModelState.AddModelError("", "Invalid credentials");
            }
            return View(signInModel);
        }
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await accountRepository.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            //await accountRepository.SignOutAsync();
            return View();
        }
        [Route("change-password")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePassword)
        {
            if (ModelState.IsValid)
            {
                var result = await accountRepository.ChangePasswordAsync(changePassword);
                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            //await accountRepository.SignOutAsync();
            return View();
        }
        [AllowAnonymous]
        [HttpGet("confirm-email/${uid}/${token}/${email}")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
        {
            EmailConfirmModel model = new EmailConfirmModel
            {
                Email = email
            };

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');
                
                var result = await accountRepository.ConfirmEmailAsync(uid, token);
                if (result.Succeeded)
                {
                    model.EmailVerified = true;
                }
            }

            return View(model);
        }
        [AllowAnonymous]
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel model)
        {
            var user = await accountRepository.GetUserByEmailAsync(model.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    model.EmailVerified = true;
                    return View(model);
                }
                string OTP = RandomString(6);
                await accountRepository.GenerateEmailConfirmationTokenAsync(user,OTP);
                model.EmailSent = true;
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong.");
            }
            return View(model);
        }


        //forgot password

        [AllowAnonymous, HttpGet("fotgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [AllowAnonymous, HttpPost("fotgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // code here
                var user = await accountRepository.GetUserByEmailAsync(model.Email);
                if (user != null)
                {
                    string OTP = RandomString(6);
                    ViewBag.forgotOTP = OTP;
                    await accountRepository.GenerateForgotPasswordTokenAsync(user, OTP);
                }

                ModelState.Clear();
                model.EmailSent = true;
            }
            return View(model);
        }

        [AllowAnonymous, HttpGet("reset-password")]
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel
            {
                Token = token,
                UserId = uid
            };
            return View(resetPasswordModel);
        }
        [AllowAnonymous, HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                model.Token = model.Token.Replace(' ', '+');
                var result = await accountRepository.ResetPasswordAsync(model);
                if (result.Succeeded)
                {
                    ModelState.Clear();
                    model.IsSuccess = true;
                    return View(model);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            // var result =accountRepository.GetProfile();
            ViewBag.AllPosts = postRepository.GetAllPosts(await userManager.GetEmailAsync(await userManager.GetUserAsync(User)));
            return View();
        }

        [HttpGet("edit-profile/${Email}")]
        public async Task<IActionResult> EditProfile(string Email)
        {
            var user = await accountRepository.EditProfile(Email);


            return View(user);

        }
        [HttpPost("edit-profile/${Email}")]
        public async Task<IActionResult> EditProfile(EditProfile model, string Email)
        {
            var result = await accountRepository.EditProfileConfirm(model);
            if (result.Succeeded)
            {

                return RedirectToAction("profile");
            }

            return View(model);

        }


        //setting
        [AllowAnonymous]
        [HttpGet]

        public async Task<IActionResult> verifyOTP()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]

        public async Task<IActionResult> verifyOTP(string NewPassword, string ConfirmNewPassword)
        {
            ViewBag.ConfirmedOTP = true;
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> emailConfirmed()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> emailConfirmed(string OTP)
        {
            ViewBag.VerifiedEmail = true;
            return View();
        }
    }
}

