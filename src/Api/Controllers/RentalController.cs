using Api.Controllers;
using Application.UseCases.Base;
using Application.UseCases.Rental.Create;
using Application.UseCases.Rental.List;
using Application.UseCases.Rental.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    /// <summary>
    /// Locação
    /// </summary>
    [Route("locacao")]
    public class RentalController : BaseController
    {
        private readonly IMediator _mediator;

        public RentalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Alugar uma moto
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<CustomResponseMessage>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(List<CustomResponseMessage>))]
        public async Task<IActionResult> PostAsync([FromBody] RentalCreateRequest request, CancellationToken cancellationToken)
        {
            var operationResult = await _mediator.Send(request, cancellationToken);
            return CreateResponse(operationResult);
        }

        /// <summary>
        /// Consultar locação por id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<CustomResponseMessage>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(List<CustomResponseMessage>))]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            RentalGetRequest request = new() { Id = id };
            var operationResult = await _mediator.Send(request, cancellationToken);
            return CreateResponse(operationResult);
        }

        /// <summary>
        /// Informar a data de devolução e calcular valor
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<CustomResponseMessage>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(List<CustomResponseMessage>))]
        public async Task<IActionResult> PutAsync([FromRoute] Guid id, [FromBody] RentalUpdateRequest request, CancellationToken cancellationToken)
        {
            request.Id = id;
            var operationResult = await _mediator.Send(request, cancellationToken);
            return CreateResponse(operationResult);
        }
    }
}
