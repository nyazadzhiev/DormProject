using DormProject.Identity.Data;
using DormProject.Identity.Models;
using DormProject.Services;

namespace DormProject.Identity.Services
{
    public interface IIdentityService
    {
        Task<Result<AppUser>> Register(UserInputModel userInput);

        Task<Result<UserOutputModel>> Login(UserInputModel userInput);

        Task<Result> ChangePassword(string userId, ChangePasswordInputModel changePasswordInput);
    }
}
