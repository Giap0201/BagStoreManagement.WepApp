using BagStore.Web.Models.Entities;
using BagStore.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BagStore.Web.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterCustomerAsync(RegisterViewModel model);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task<ApplicationUser?> GetProfileAsync(string userId);

        Task<IdentityResult> UpdateProfileAsync(ProfileEditViewModel model);

        Task<IdentityResult> DeleteAccountAsync(string userId, string currentPassword);

        Task LogoutAsync();

        Task<List<ApplicationUser>> GetAllCustomersAsync();

        Task<ApplicationUser?> GetByIdAsync(string id);

        Task<IdentityResult> CreateCustomerAsync(AdminCustomerViewModel model);

        Task<IdentityResult> UpdateCustomerAsync(AdminCustomerViewModel model);

        Task<IdentityResult> DeleteAccountAsync(string userId);

        Task<IdentityResult> ResetPasswordAsync(string userId, string newPassword);

        //API
        Task<string?> GenerateJwtForUserAsync(string userName, string password);

        Task<ApplicationUser?> GetProfileByUserNameAsync(string username);

        string GenerateJwtToken(ApplicationUser user);

        Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
    }
}