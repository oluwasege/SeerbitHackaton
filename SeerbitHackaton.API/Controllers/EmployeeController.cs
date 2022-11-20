using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Pagination;

namespace SeerbitHackaton.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPut]
        [Authorize(Roles = AppRoles.Employee)]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        public async Task<IActionResult> UpdateEmployeeInformation([FromBody] UpdateEmployeeRequest model)
        {

            if (!ModelState.IsValid)
                return ApiResponse<ResultModel<string>>(errors: ListModelErrors.ToArray(), codes: ApiResponseCodes.INVALID_REQUEST);
            try
            {
                var result = await _employeeService.UpdateEmployee(model);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<string>(null, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.Employee)]
        [ProducesResponseType(typeof(ApiResponse<EmployeeResponse>), 200)]
        public async Task<IActionResult> GetEmployee()
        {

            try
            {
                var result = await _employeeService.GetEmployee();

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<EmployeeResponse>(null, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }


        [HttpGet()]
        [Authorize(Roles = AppRoles.CompanyAdmin)]
        [ProducesResponseType(typeof(ApiResponse<PaginatedModel<EmployeeResponse>>), 200)]
        public async Task<IActionResult> GetAllEmployeesForCompanyAdmin([FromQuery] long? companyId, QueryModel model)
        {
            var result = await _employeeService.GetAllEmployees(companyId, model, false);

            if (result.HasError)
                return ApiResponse<string>(errors: result.ErrorMessages.ToArray());

            return ApiResponse(message: result.Message, codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.TotalItemCount);
        }


        [HttpGet()]
        [Authorize(Roles = AppRoles.SuperAdmin)]
        [ProducesResponseType(typeof(ApiResponse<PaginatedModel<EmployeeResponse>>), 200)]
        public async Task<IActionResult> GetAllEmployeesForSuperAdmin([FromQuery] long? companyId, QueryModel model)
        {
            var result = await _employeeService.GetAllEmployees(companyId, model, true);

            if (result.HasError)
                return ApiResponse<string>(errors: result.ErrorMessages.ToArray());

            return ApiResponse(message: result.Message, codes: ApiResponseCodes.OK, data: result.Data, totalCount: result.Data.TotalItemCount);
        }

    }
}
