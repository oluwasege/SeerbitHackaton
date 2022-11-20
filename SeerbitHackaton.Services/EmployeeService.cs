using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SeerbitHackaton.Core.AspnetCore;
using SeerbitHackaton.Core.DataAccess.EfCore.UnitOfWork;
using SeerbitHackaton.Core.DataAccess.Repository;
using SeerbitHackaton.Core.Entities;
using SeerbitHackaton.Core.Timing;
using Shared.Pagination;

namespace SeerbitHackaton.Services
{
    public class EmployeeService
    {
        private readonly IRepository<Employee, long> _employeeRepository;
        private readonly IRepository<Company, long> _companyRepository;
        ILogger<EmployeeService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        public EmployeeService(IRepository<Employee, long> employeeRepository,
                               IUnitOfWork unitOfWork,
                               IHttpUserService currentUserService,
                               ILogger<EmployeeService> logger,
                               IRepository<Company, long> companyRepository,
                               UserManager<User> userManager)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _companyRepository = companyRepository;
            _userManager = userManager;
        }

        public async Task<ResultModel<EmployeeResponse>> GetEmployee()
        {
            var resultModel = new ResultModel<EmployeeResponse>();
            try
            {
                var userId = GetCurrentUserId();
                var employee = await _employeeRepository.GetAll().Include(x => x.Company).FirstOrDefaultAsync(x => x.UserId == userId);
                resultModel.Data = employee;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message ?? ex.InnerException.Message}");
                resultModel.AddError(ex.Message);
                return resultModel;
            }
            return resultModel;
        }

        public async Task<ResultModel<PaginatedModel<EmployeeResponse>>> GetAllEmployees(long companyId, QueryModel model, bool isSuperAdmin)
        {
            var resultModel = new ResultModel<PaginatedModel<EmployeeResponse>>();
            try
            {
                var query = _employeeRepository.GetAllIncluding(x => x.Company);
                if (isSuperAdmin == false)
                    query = query.Where(x => x.CompanyId == companyId);

                var employees = await query.ToPagedListAsync(model.PageIndex, model.PageSize);
                var pagedEmployees = employees.Select(x => (EmployeeResponse)x).ToList();
                var data = new PaginatedModel<EmployeeResponse>(pagedEmployees, model.PageIndex, model.PageSize, query.Count());
                resultModel.Data = data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message ?? ex.InnerException.Message}");
                resultModel.AddError(ex.Message);
                return resultModel;
            }

            return resultModel;

        }

        public async Task<ResultModel<string>> UpdateEmployee(UpdateEmployeeRequest model)
        {
            var resultModel = new ResultModel<string>();
            try
            {
                var userId = GetCurrentUserId();
                var employee = await GetEmployee(userId);
                if (employee == null)
                {
                    resultModel.AddError("Employee does not exist");
                    return resultModel;
                }
                var date = Clock.Now;

                employee.User.DateOfBirth = string.IsNullOrWhiteSpace(model.DateOfBirth) ? employee.User.DateOfBirth : Convert.ToDateTime(model.DateOfBirth);
                employee.User.FirstName = string.IsNullOrWhiteSpace(model.FirstName) ? employee.User.FirstName : model.FirstName;
                employee.User.LastName=string.IsNullOrWhiteSpace(model.LastName) ? employee.User.LastName : model.LastName;
                employee.User.MiddleName=string.IsNullOrWhiteSpace(model.MiddleName) ? employee.User.MiddleName : model.MiddleName;
                employee.User.PhoneNumber = string.IsNullOrWhiteSpace(model.PhoneNumber) ? employee.User.PhoneNumber : model.PhoneNumber;
                employee.AccountNumber = string.IsNullOrWhiteSpace(model.AccountNumber) ? employee.AccountNumber : model.AccountNumber;
                employee.BankName=string.IsNullOrWhiteSpace(model.BankName)?employee.BankName: model.BankName;
                employee.LastModificationTime = date;
                employee.LastModifierUserId = userId;
                employee.User.LastModificationTime= date;
                await _userManager.UpdateAsync(employee.User);
                await _employeeRepository.UpdateAsync(employee);
                await _unitOfWork.SaveChangesAsync();
                resultModel.Data = "Success";

            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError($"{ex.Message ?? ex.InnerException.Message}");
                resultModel.AddError(ex.Message);
                return resultModel;
            }
            return resultModel;
        }
        private long GetCurrentUserId() => _currentUserService.GetCurrentUser().UserId;
        private async Task<Employee> GetEmployee(long userId) => await _employeeRepository.GetAll().Include(x => x.Company).Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
