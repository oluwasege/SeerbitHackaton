global using SeerbitHackaton.Core.Enums;
global using SeerbitHackaton.Core.ViewModels.UserViewModel;
global using SeerbitHackaton.Core.ViewModels;
global using SeerbitHackaton.Services.Interfaces;
global using Microsoft.AspNetCore.Authorization;

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

            try
            {
                var result = await _payrollService.BulkUpload(model);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<LoginResponseVM>(null, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
