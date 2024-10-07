using Application.UseCases.Base;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Motorcycle.Get
{
    public class MotorcycleGetHandler : IRequestHandler<MotorcycleGetRequest, CustomResponse>, IDisposable
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public MotorcycleGetHandler(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<CustomResponse> Handle(MotorcycleGetRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Motorcycle entity = await _motorcycleRepository.GetAsync(x => x.Id == request.Id, cancellationToken);
            if (entity is null)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Response = CustomResponseMessage.FromMessage("Identificador não localizado."),
                };
            }

            return new CustomResponse
            {
                Success = true,
                HttpStatusCode = HttpStatusCode.OK,
                Response = MotorcycleGetResponse.FromEntity(entity)
            };
        }

        public void Dispose()
        {
            _motorcycleRepository?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
