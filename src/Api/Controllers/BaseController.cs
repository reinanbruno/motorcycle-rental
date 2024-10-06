using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Base;

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
