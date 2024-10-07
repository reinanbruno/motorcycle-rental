using Api.Controllers;
using Application.UseCases.Base;
using Application.UseCases.Motorcycle.Create;
using Application.UseCases.Motorcycle.Delete;
using Application.UseCases.Motorcycle.Get;
using Application.UseCases.Motorcycle.List;
using Application.UseCases.Motorcycle.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    /// <summary>
    /// Motos
    /// </summary>
    [Route("motos")]
    public class MotorcycleController : BaseController
    {
        private readonly IMediator _mediator;

        public MotorcycleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Cadastrar uma nova moto
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<CustomResponseMessage>))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(List<CustomResponseMessage>))]
        public async Task<IActionResult> PostAsync([FromBody] MotorcycleCreateRequest request, CancellationToken cancellationToken)
        {
            var operationResult = await _mediator.Send(request, cancellationToken);
            return CreateResponse(operationResult);
        }

        /// <summary>
        /// Modificar a placa de uma moto
        /// </summary>
        [HttpPut("{id}/placa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<CustomResponseMessage>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(List<CustomResponseMessage>))]
        public async Task<IActionResult> PutAsync([FromRoute] Guid id, [FromBody] MotorcycleUpdateRequest request, CancellationToken cancellationToken)
        {
            request.Id = id;
            var operationResult = await _mediator.Send(request, cancellationToken);
            return CreateResponse(operationResult);
        }

        /// <summary>
        /// Remover uma moto
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<CustomResponseMessage>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(List<CustomResponseMessage>))]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            MotorcycleDeleteRequest request = new() { Id = id };
            var operationResult = await _mediator.Send(request, cancellationToken);
            return CreateResponse(operationResult);
        }

        /// <summary>
        /// Consultar motos existentes por id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<CustomResponseMessage>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(List<CustomResponseMessage>))]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            MotorcycleGetRequest request = new() { Id = id };
            var operationResult = await _mediator.Send(request, cancellationToken);
            return CreateResponse(operationResult);
        }


        /// <summary>
        /// Consultar motos existentes
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ListAsync([FromQuery(Name = "placa")] string plate, CancellationToken cancellationToken)
        {
            MotorcycleListRequest request = new() { Plate = plate };
            var operationResult = await _mediator.Send(request, cancellationToken);
            return CreateResponse(operationResult);
        }
    }
}
