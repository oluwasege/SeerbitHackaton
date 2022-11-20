using Microsoft.AspNetCore.Identity;
using SeerbitHackaton.Core.DataAccess.EfCore.UnitOfWork;
using SeerbitHackaton.Core.Timing;
using Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<ResultModel<EmployeeResponse>> GetEmployee();

        Task<ResultModel<PaginatedModel<EmployeeResponse>>> GetAllEmployees(long? companyId, QueryModel model, bool isSuperAdmin);

        Task<ResultModel<string>> UpdateEmployee(UpdateEmployeeRequest model);
    }
}
