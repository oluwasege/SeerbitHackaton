using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeerbitHackaton.Core.AspnetCore;
using SeerbitHackaton.Core.Enums;
using SeerbitHackaton.Core.ViewModels.UserViewModel;
using SeerbitHackaton.Core.ViewModels;
using SeerbitHackaton.Services.Interfaces;
using SeerbitHackaton.Core.Timing;
using Microsoft.AspNetCore.Authorization;
using SeerbitHackaton.Core.Entities;

namespace SeerbitHackaton.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authService;
        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseVM>), 200)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginVM model)
        {

            try
            {
                var result = await _authService.LoginAsync(model, Clock.Now);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<LoginResponseVM>(null, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseVM>), 200)]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest model)
        {

            try
            {
                var result = await _authService.CreateEmployee(model);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<LoginResponseVM>(null, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok("working");
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordVM model)
        {

            try
            {
                var result = await _authService.ResetPassword(model, CurrentUser.UserId, Clock.Now);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<bool>(false, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
        public async Task<IActionResult> ConfirmEmailAndChangePasswordAsync([FromBody] ChangePasswordVM model)
        {

            try
            {
                var result = await _authService.ChangePasswordAsync(model, Clock.Now);

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<bool>(false, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                // _log.LogInformation(ex.InnerException, ex.Message);

                return HandleError(ex);
            }
        }
    }
}
