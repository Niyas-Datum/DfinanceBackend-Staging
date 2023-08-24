using Dfinance.Shared.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Dfinance.api.Framework
{
    public class BaseController : Controller
    {
        protected new IActionResult Response(CommonResponse response)
        {
            var returnResult = response.IsValid ? response.Data : new { Error = response.Exception.Message };
            return new ObjectResult(returnResult) { StatusCode = response.HttpCode };
        }
    }
}