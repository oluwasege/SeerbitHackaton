using Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Services.Interfaces
{
    public interface IPayrollService
    {
        Task<ResultModel<string>> BulkUpload(UploadEmployeesPayrollVM model);
        Task<ResultModel<PaginatedModel<PayrollResponse>>> GetAllPayrolls(long? employeeId, long? companyId, QueryModel model, bool isCompanyAdmin);
    }
}
