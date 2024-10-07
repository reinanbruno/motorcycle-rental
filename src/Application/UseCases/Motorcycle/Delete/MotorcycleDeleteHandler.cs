using Application.UseCases.Base;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Motorcycle.Delete
{
    public class MotorcycleDeleteHandler : IRequestHandler<MotorcycleDeleteRequest, CustomResponse>, IDisposable
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MotorcycleDeleteHandler(IMotorcycleRepository motorcycleRepository, IRentalRepository rentalRepository, IUnitOfWork unitOfWork)
        {
            _motorcycleRepository = motorcycleRepository;
            _rentalRepository = rentalRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomResponse> Handle(MotorcycleDeleteRequest request, CancellationToken cancellationToken)
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

            if (await _rentalRepository.GetAsync(x => x.MotorcycleId == request.Id, cancellationToken) is not null)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Response = CustomResponseMessage.FromMessage("Não é possível excluir essa moto pois ela tem locações."),
                };
            }

            _motorcycleRepository.Delete(entity);
            if (!await _unitOfWork.Commit(cancellationToken))
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Response = CustomResponseMessage.FromMessage("Houve um problema ao persistir suas informações. Por favor, tente novamente em instantes."),
                };
            }

            return new CustomResponse
            {
                Success = true,
                HttpStatusCode = HttpStatusCode.OK
            };
        }

        public void Dispose()
        {
            _motorcycleRepository?.Dispose();
            _unitOfWork?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
