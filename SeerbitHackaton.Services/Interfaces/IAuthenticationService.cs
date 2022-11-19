global using SeerbitHackaton.Core.ViewModels;
global using SeerbitHackaton.Core.ViewModels.UserViewModel;

namespace SeerbitHackaton.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ResultModel<LoginResponseVM>> LoginAsync(LoginVM model, DateTime currentDate);
        //Task<ResultModel<bool>> ResetPassword(ResetPasswordVM model, string userId, DateTime currentDate);
        //Task<ResultModel<LoginResponseVM>> RefreshTokenAsync(string token, DateTime date);
        //Task<ResultModel<bool>> InviteUser(RegisterUserVM model);
        //Task<ResultModel<PaginatedList<UserVM>>> GetAllUsers(BaseSearchViewModel model);
        //Task<ResultModel<UserVM>> GetUserAsync(string email);
        //Task<ResultModel<string>> AssignUserToRole(string email, string role, string CurrentUserID);
        //Task<ResultModel<List<RoleVm>>> GetAllRoles();
        //Task<ResultModel<bool>> ChangePasswordAsync(ChangePasswordVM model, DateTime currentDate);




    }
}
