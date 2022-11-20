using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SeerbitHackaton.Core.AspnetCore;
using SeerbitHackaton.Core.DataAccess.EfCore.UnitOfWork;
using SeerbitHackaton.Core.DataAccess.Repository;
using SeerbitHackaton.Core.Entities;
using Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Services
{
    public class CompanyAdminService
    {
        private readonly IRepository<CompanyAdmin, long> _companyAdminRepository;
        private readonly IRepository<Company, long> _companyRepository;
        private readonly IRepository<Payroll, long> _payrollRepository;
        ILogger<CompanyAdminService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpUserService _currentUserService;
        private readonly IConfiguration _configuration;
        public CompanyAdminService(IRepository<CompanyAdmin, long> companyAdminRepository,
                               IUnitOfWork unitOfWork,
                               IHttpUserService currentUserService,
                               ILogger<CompanyAdminService> logger,
                               IRepository<Company, long> companyRepository,
                               IRepository<Payroll, long> payrollRepository)
        {
            _companyAdminRepository = companyAdminRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _companyRepository = companyRepository;
            _payrollRepository = payrollRepository;
        }

        public async Task<ResultModel<CompanyAdminResponse>> GetCompanyAdmin()
        {
            var resultModel = new ResultModel<CompanyAdminResponse>();
            try
            {
                var userId = GetCurrentUserId();
                var companyAdmin = await _companyAdminRepository.GetAll().Include(x => x.Company).FirstOrDefaultAsync(x => x.UserId == userId);
                resultModel.Data = companyAdmin;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message ?? ex.InnerException.Message}");
                resultModel.AddError(ex.Message);
                return resultModel;
            }
            return resultModel;
        }

        public async Task<ResultModel<PaginatedModel<CompanyAdminResponse>>> GetAllCompanyAdmins(long companyId, QueryModel model, bool isSuperAdmin)
        {
            var resultModel = new ResultModel<PaginatedModel<CompanyAdminResponse>>();
            try
            {
                var query = _companyAdminRepository.GetAllIncluding(x => x.Company);
                if (isSuperAdmin == false)
                    query = query.Where(x => x.CompanyId == companyId);

                var employees = await query.ToPagedListAsync(model.PageIndex, model.PageSize);
                var pagedEmployees = employees.Select(x => (CompanyAdminResponse)x).ToList();
                var data = new PaginatedModel<CompanyAdminResponse>(pagedEmployees, model.PageIndex, model.PageSize, query.Count());
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
