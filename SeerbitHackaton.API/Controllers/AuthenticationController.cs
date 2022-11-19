using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeerbitHackaton.Core.AspnetCore;
using SeerbitHackaton.Core.Enums;
using SeerbitHackaton.Core.ViewModels.UserViewModel;
using SeerbitHackaton.Core.ViewModels;
using SeerbitHackaton.Services.Interfaces;
using SeerbitHackaton.Core.Timing;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok("working");
        }
    }
}
