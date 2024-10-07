using Application.UseCases.Base;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class BaseController : Controller
    {
        protected IActionResult CreateResponse(CustomResponse operationResult)
        {
            return StatusCode((int)operationResult.HttpStatusCode, operationResult.Response);
        }
    }
}
