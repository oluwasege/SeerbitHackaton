using Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Services.Interfaces
{
    public interface ICompanyAdminServices
    {
        Task<ResultModel<CompanyAdminResponse>> GetCompanyAdmin();

        Task<ResultModel<PaginatedModel<CompanyAdminResponse>>> GetAllCompanyAdmins(long companyId, QueryModel model, bool isSuperAdmin);
    }
}
