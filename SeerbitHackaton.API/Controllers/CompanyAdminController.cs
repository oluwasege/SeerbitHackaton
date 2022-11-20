using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SeerbitHackaton.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyAdminController : BaseController
    {
        private readonly ICompanyAdminServices _companyAdminServices;
        public CompanyAdminController(ICompanyAdminServices companyAdminServices)
        {
            _companyAdminServices = companyAdminServices;
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.CompanyAdmin)]
        [ProducesResponseType(typeof(ApiResponse<CompanyAdminResponse>), 200)]
        public async Task<IActionResult> GetEmployee()
        {

            try
            {
                var result = await _companyAdminServices.GetCompanyAdmin();

                if (!result.HasError)
                    return ApiResponse(result.Data, message: result.Message, ApiResponseCodes.OK);

                return ApiResponse<CompanyAdminResponse>(null, message: result.Message, ApiResponseCodes.FAILED, errors: result.GetErrorMessages().ToArray());
            }
            catch (Exception ex)
            {
                return HandleError(ex);
            }
        }
    }
}
