global using SeerbitHackaton.Core.Enums;
global using SeerbitHackaton.Core.ViewModels.UserViewModel;
global using SeerbitHackaton.Core.ViewModels;
global using SeerbitHackaton.Services.Interfaces;
global using Microsoft.AspNetCore.Authorization;
using Shared.Pagination;

namespace SeerbitHackaton.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PayrollController : BaseController
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }


        [HttpPost]
        [Authorize(Roles = AppRoles.CompanyAdmin)]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UploadPayroll([FromForm] UploadEmployeesPayrollVM model)
        {

            if (!ModelState.IsValid)
                return ApiResponse<ResultModel<string>>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            try
            {
                var result = await _payrollService.BulkUpload(model);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<string>(null, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpGet()]
        [Authorize(Roles = AppRoles.Employee)]
        [ProducesResponseType(typeof(ApiResponse<PaginatedModel<PayrollResponse>>), 200)]
        public async Task<IActionResult> GetAllPayrollForEmployee([FromQuery] long? employeeId, [FromQuery] long? companyId, [FromQuery] QueryModel model)
        {
            var result = await _payrollService.GetAllPayrolls(employeeId, companyId, model, false);

            if (result.HasError)
                return ApiResponse<string>(errors: result.ErrorMessages.ToArray());

            return ApiResponse(message: result.Message, codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.TotalItemCount);
        }

        [HttpGet()]
        [Authorize(Roles = AppRoles.CompanyAdmin)]
        [ProducesResponseType(typeof(ApiResponse<PaginatedModel<PayrollResponse>>), 200)]
        public async Task<IActionResult> GetAllPayrollForCompanyAdmin([FromQuery] long? employeeId, [FromQuery] long? companyId, [FromQuery] QueryModel model)
        {
            var result = await _payrollService.GetAllPayrolls(employeeId, companyId, model, false);

            if (result.HasError)
                return ApiResponse<string>(errors: result.ErrorMessages.ToArray());

            return ApiResponse(message: result.Message, codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.TotalItemCount);
        }


    }
}
