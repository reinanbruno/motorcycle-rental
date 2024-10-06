using MediatR;
using Application.UseCases.Base;
using Application.UseCases.Motorcycle.Get;
using Domain.Repositories;
using System.Net;

namespace Application.UseCases.Motorcycle.List
{
    public class MotorcycleListHandler : IRequestHandler<MotorcycleListRequest, CustomResponse>, IDisposable
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public MotorcycleListHandler(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public async Task<CustomResponse> Handle(MotorcycleListRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<Domain.Entities.Motorcycle> entities = await _motorcycleRepository.ListAsync(x => x.Plate == request.Plate, cancellationToken);
            
            return new CustomResponse
            {
                Success = true,
                HttpStatusCode = HttpStatusCode.OK,
                Response = entities.Select(e => MotorcycleGetResponse.FromEntity(e))
            };
        }

        public void Dispose()
        {
            _motorcycleRepository?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
