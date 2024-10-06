using MediatR;
using Application.UseCases.Base;
using Domain.Repositories;
using System.Net;

namespace Application.UseCases.Motorcycle.Update
{
    public class MotorcycleUpdateHandler : IRequestHandler<MotorcycleUpdateRequest, CustomResponse>, IDisposable
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MotorcycleUpdateHandler(IMotorcycleRepository motorcycleRepository, IUnitOfWork unitOfWork)
        {
            _motorcycleRepository = motorcycleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomResponse> Handle(MotorcycleUpdateRequest request, CancellationToken cancellationToken)
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

            if (await _motorcycleRepository.GetAsync(x => x.Plate == request.Plate, cancellationToken) is not null)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.Conflict,
                    Response = CustomResponseMessage.FromMessage("Essa placa já está em uso."),
                };
            }

            entity.Plate = request.Plate;
            _motorcycleRepository.Update(entity);
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
