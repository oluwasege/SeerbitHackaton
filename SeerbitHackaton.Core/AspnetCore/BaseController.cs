using log4net;
using Microsoft.AspNetCore.Mvc;
using SeerbitHackaton.Core.AspnetCore.Identity;
using SeerbitHackaton.Core.Extensions;
using System.Runtime.CompilerServices;

namespace SeerbitHackaton.Core.AspnetCore
{
    //[AllowAnonymous]
    public class BaseController : ControllerBase
    {
        private readonly ILog _logger;

        public BaseController()
        {
            _logger = LogManager.GetLogger(typeof(BaseController));
        }

        protected UserPrincipal CurrentUser
        {
            get
            {
                return new UserPrincipal(User as ClaimsPrincipal);
            }
        }

        /// <summary>
        /// Read ModelError into string collection
        /// </summary>
        /// <returns></returns>
        protected List<string> ListModelErrors
        {
            get
            {
                return ModelState.Values
                  .SelectMany(x => x.Errors
                    .Select(ie => ie.ErrorMessage))
                    .ToList();
            }
        }

        protected IActionResult HandleError(Exception ex, string customErrorMessage = null)
        {
            _logger.Error(ex.StackTrace, ex);

            var rsp = new ApiResponse<string>();
            rsp.Code = ApiResponseCodes.ERROR;
#if DEBUG
            rsp.Errors.Add($"Error: {ex?.InnerException?.Message ?? ex.Message} --> {ex?.StackTrace}");
            return Ok(rsp);
#else
            rsp.Errors.Add(customErrorMessage ?? "An error occurred while processing your request!");
            return Ok(rsp);
#endif
        }

        public IActionResult ApiResponse<T>(T data = default, string message = "",
            ApiResponseCodes codes = ApiResponseCodes.OK, int? totalCount = 0, params string[] errors)
        {
            var response = new ApiResponse<T>(data, message, codes, totalCount, errors);
            response.Description = message ?? response.Code.GetDescription();
            return Ok(response);
        }

        protected async Task<ApiResponse<T>> HandleApiOperationAsync
            <T>
            (
           Func<Task<ApiResponse<T>>> action,
           [CallerLineNumber] int lineNo = 0,
           [CallerMemberName] string method = "")
        {
            var apiResponse = new ApiResponse<T>
            {
                Code = ApiResponseCodes.OK
            };

            try
            {
                var methodResponse = await action.Invoke();

                apiResponse.ResponseCode = methodResponse.ResponseCode;
                apiResponse.Payload = methodResponse.Payload;
                apiResponse.TotalCount = methodResponse.TotalCount;
                apiResponse.Code = methodResponse.Code;
                apiResponse.Errors = methodResponse.Errors;
                apiResponse.Description = string.IsNullOrEmpty(apiResponse.Description) ? methodResponse.Description : apiResponse.Description;

                return apiResponse;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace, ex);
                apiResponse.Code = ApiResponseCodes.EXCEPTION;

#if DEBUG
                apiResponse.Description = $"Error: {ex?.InnerException?.Message ?? ex.Message} --> {ex?.StackTrace}";
#else
                apiResponse.Description = "System error occurred. Please contact application support!";
#endif
                apiResponse.Errors.Add(apiResponse.Description);
                return apiResponse;
            }
        }
    }
}