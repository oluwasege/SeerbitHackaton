using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

            try
            {
                var result = await _employeeService.UpdateEmployee(model);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<LoginResponseVM>(null, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
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

                return ApiResponse<LoginResponseVM>(null, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
