using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SeerbitHackaton.Core.AspnetCore;
using SeerbitHackaton.Core.DataAccess.EfCore.UnitOfWork;
using SeerbitHackaton.Core.DataAccess.Repository;
using SeerbitHackaton.Core.Entities;
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
        public EmployeeService(IRepository<Employee, long> employeeRepository,
                               IUnitOfWork unitOfWork,
                               IHttpUserService currentUserService,
                               ILogger<EmployeeService> logger,
                               IRepository<Company, long> companyRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _companyRepository = companyRepository;
        }

        public async Task<ResultModel<EmployeeResponse>> GetEmployee()
        {
            var resultModel = new ResultModel<EmployeeResponse>();
            try
            {
                var employeeId = GetCurrentUserId();
                var employee = _employeeRepository.GetAll().Include(x=>x.Company).FirstOrDefault(x => x.Id == employeeId);
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
                var query = _employeeRepository.GetAllIncluding(x=>x.Company);
                if (isSuperAdmin == false)
                    query = query.Where(x => x.CompanyId == companyId);

                var employees = await query.ToPagedListAsync(model.PageIndex, model.PageSize);
                var pagedEmployees = employees.Select(x => (EmployeeResponse)x).ToList();
                foreach (var item in pagedEmployees)
                {

                }
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
        private long GetCurrentUserId() => _currentUserService.GetCurrentUser().UserId;
    }
}
