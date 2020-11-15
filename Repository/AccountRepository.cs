using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MII_Media.Data;
using MII_Media.Models;
using MII_Media.Service;
using MII_Media.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.Repository
{
   
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserService userService;
        private readonly IConfiguration configuration;
        private readonly IEmailService emailService;

       // private MiiContext context = new MiiContext();
        public AccountRepository(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,
            IUserService userService,IConfiguration configuration,IEmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            this.configuration = configuration;
            this.emailService = emailService;
        }
        public async Task<IdentityResult> CreateUserAsync(SignUpUserModel usermodel)
        {
            var user = new ApplicationUser()
            {

                FirstName = usermodel.FirstName,
                LastName = usermodel.LastName,
                DOB = usermodel.DOB,
                UserName = usermodel.Email,
                Email = usermodel.Email,
                PhoneNumber = usermodel.PhoneNumber,
            };
            var result = await userManager.CreateAsync(user, usermodel.Password);
            if (result.Succeeded)
            {
                await GenerateEmailConfirmationTokenAsync(user);
            }
            return result;
        }
        public async Task<SignInResult> PasswordSignInAsync(SignInModel signInModel)
        {
            var result = await signInManager.PasswordSignInAsync(signInModel.Email,signInModel.Password,signInModel.RememberMe,false);
                return result;
        }
        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel changePassword)
        {
            var userId = userService.GetUserId();
            var user = await userManager.FindByIdAsync(userId);
            return await userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);
            
        }


        //email

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }
        public async Task GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendEmailConfirmationEmail(user, token);
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
        {
            return await userManager.ConfirmEmailAsync(await userManager.FindByIdAsync(uid), token);
        }

        private async Task SendEmailConfirmationEmail(ApplicationUser user, string token)
        {
            string appDomain = configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = configuration.GetSection("Application:EmailConfirmation").Value;

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };

            await emailService.SendEmailForEmailConfirmation(options);
        }

        //forgot passwowrd

        public async Task GenerateForgotPasswordTokenAsync(ApplicationUser user)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            if (!string.IsNullOrEmpty(token))
            {
                await SendForgotPasswordEmail(user, token);
            }
        }
        private async Task SendForgotPasswordEmail(ApplicationUser user, string token)
        {
            string appDomain = configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = configuration.GetSection("Application:ForgotPassword").Value;

            UserEmailOptions options = new UserEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };

            await emailService.SendEmailForForgotPassword(options);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            return await userManager.ResetPasswordAsync(await userManager.FindByIdAsync(model.UserId), model.Token, model.NewPassword);
        }

        public async Task<EditProfile> EditProfile(string email)

        {
            ApplicationUser appUser = new ApplicationUser();
            appUser =await userManager.FindByEmailAsync(email);
            EditProfile user = new EditProfile();
           
            user.FirstName = appUser.FirstName;
            user.LastName = appUser.LastName;
            user.Email = appUser.Email;
            user.DOB = appUser.DOB;
            user.PhoneNumber = appUser.PhoneNumber;
            user.Bio = appUser.Bio;
            return user ;
        }

        public async Task<IdentityResult> EditProfileConfirm(EditProfile model)
        {
            ApplicationUser currentUser = new ApplicationUser();
            currentUser = await userManager.FindByEmailAsync(model.Email); ;
            currentUser.FirstName = model.FirstName;
            currentUser.LastName = model.LastName;
            currentUser.PhoneNumber = model.PhoneNumber;
            currentUser.DOB = model.DOB;
            currentUser.Bio = model.Bio;
            
           var result= await userManager.UpdateAsync(currentUser);

            return result;
        }
    }
}
