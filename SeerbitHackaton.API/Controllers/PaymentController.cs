global using Microsoft.AspNetCore.Mvc;
global using SeerbitHackaton.Core.AspnetCore;
using System.Net;

namespace SeerbitHackaton.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : BaseController
    {
        //private readonly IPaymentMethod _paymentMethod;
        //public PaymentController(IPaymentMethod paymentMethod)
        //{
        //    _paymentMethod = paymentMethod;
        //}

        //[HttpGet]
        ////[Route("Banks/{token}")]
        //public async Task<IActionResult> Banks(string token)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var result = await _paymentMethod.GetBanks(token);
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadGateway);
        //        response.ReasonPhrase = ex.Message;
        //        return BadRequest(response);
        //    }
        //}
    }
}
