using Microsoft.AspNetCore.Identity;
using MII_Media.Models;
using MII_Media.ViewModels;
using System.Threading.Tasks;

namespace MII_Media.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUpUserModel usermodel,string OTP);

       Task<SignInResult> PasswordSignInAsync(SignInModel signInModel);

        Task SignOutAsync();

        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel changePassword);

        Task GenerateEmailConfirmationTokenAsync(ApplicationUser user, string OTP);

        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);

       // Task SendEmailConfirmationEmail(ApplicationUser user, string token);

        Task<ApplicationUser> GetUserByEmailAsync(string email);

        Task GenerateForgotPasswordTokenAsync(ApplicationUser user, string OTP);

        Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model);


        Task<EditProfile> EditProfile(string email);
        Task<IdentityResult> EditProfileConfirm(EditProfile model);
    }
}