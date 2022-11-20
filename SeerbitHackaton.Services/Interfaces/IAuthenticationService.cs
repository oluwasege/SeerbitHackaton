global using SeerbitHackaton.Core.ViewModels;
global using SeerbitHackaton.Core.ViewModels.UserViewModel;
using Microsoft.AspNetCore.Identity;
using SeerbitHackaton.Core.DataAccess.EfCore.UnitOfWork;
using SeerbitHackaton.Core.Entities;
using SeerbitHackaton.Core.Timing;
using Shared.Pagination;

namespace SeerbitHackaton.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ResultModel<LoginResponseVM>> LoginAsync(LoginVM model, DateTime currentDate);
        Task<ResultModel<bool>> ChangePasswordAsync(ChangePasswordVM model, DateTime currentDate);
        Task<ResultModel<LoginResponseVM>> RefreshTokenAsync(string token, DateTime date);
        Task<ResultModel<long>> InviteUser(RegisterUserVM model);
        Task<ResultModel<PaginatedModel<UserVM>>> GetAllUsers(QueryModel model);
        Task<ResultModel<UserVM>> GetUserAsync(string email);
        Task<ResultModel<string>> AssignUserToRole(string email, string role, string CurrentUserID);
        Task<ResultModel<List<RoleVm>>> GetAllRoles();
        Task<ResultModel<bool>> ResetPassword(ResetPasswordVM model, long userId, DateTime currentDate);

        Task<ResultModel<string>> CreateEmployee(CreateEmployeeRequest model);

        Task<ResultModel<string>> CreateCompany(CreateCompanyRequest model);

        Task<ResultModel<string>> CreateCompanyAdmin(CreateCompanyAdminRequest model);

        Task<ResultModel<string>> UpdateUser(UpdateUserRequest model);
    }
}
