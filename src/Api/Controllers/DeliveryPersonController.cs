using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Base;
using Application.UseCases.DeliveryPerson.Create;
using Application.UseCases.DeliveryPerson.Update;
using Application.UseCases.Motorcycle.Create;
using Api.Controllers;

namespace Controllers
{
    /// <summary>
    /// Entregadores
    /// </summary>
    [Route("entregadores")]
    public class DeliveryPersonController : BaseController
    {
        private IMediator _mediator;

        public DeliveryPersonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Cadastrar entregador
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<CustomResponseMessage>))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(List<CustomResponseMessage>))]
        public async Task<IActionResult> PostAsync([FromBody] DeliveryPersonCreateRequest request, CancellationToken cancellationToken)
        {
            var operationResult = await _mediator.Send(request, cancellationToken);
            return CreateResponse(operationResult);
        }

        /// <summary>
        /// Cadastrar entregador
        /// </summary>
        [HttpPost("{id}/cnh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<CustomResponseMessage>))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(List<CustomResponseMessage>))]
        public async Task<IActionResult> PostDriverLicenseImageAsync([FromRoute] Guid id, [FromBody] DeliveryPersonUpdateRequest request, CancellationToken cancellationToken)
        {
            request.Id = id;
            var operationResult = await _mediator.Send(request, cancellationToken);
            return CreateResponse(operationResult);
        }
    }
}
