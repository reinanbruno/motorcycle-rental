using Application.UseCases.Base;
using Domain.Adapters;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Motorcycle.Create
{
    public class MotorcycleCreateHandler : IRequestHandler<MotorcycleCreateRequest, CustomResponse>, IDisposable
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageBrokerAdapter _messageBrokerAdapter;

        public MotorcycleCreateHandler(IMotorcycleRepository motorcycleRepository, IUnitOfWork unitOfWork, IMessageBrokerAdapter messageBrokerAdapter)
        {
            _motorcycleRepository = motorcycleRepository;
            _unitOfWork = unitOfWork;
            _messageBrokerAdapter = messageBrokerAdapter;
        }

        public async Task<CustomResponse> Handle(MotorcycleCreateRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Motorcycle motorcycle = await _motorcycleRepository.GetAsync(x => 
                x.Id == request.Id ||
                x.Plate == request.Plate, 
                cancellationToken
            );

            if (motorcycle?.Id == request.Id)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.Conflict,
                    Response = CustomResponseMessage.FromMessage("Esse identificador já está em uso."),
                };
            }

            if (motorcycle?.Plate == request.Plate)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.Conflict,
                    Response = CustomResponseMessage.FromMessage("Essa placa já está em uso."),
                };
            }

            Domain.Entities.Motorcycle entity = request.ToEntity();
            await _motorcycleRepository.CreateAsync(entity);
            if (!await _unitOfWork.Commit(cancellationToken))
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Response = CustomResponseMessage.FromMessage("Houve um problema ao persistir suas informações. Por favor, tente novamente em instantes."),
                };
            }

            if (entity.FabricationYear == 2024)
                _messageBrokerAdapter.PublishQueue("motorcycle-created", entity);

            return new CustomResponse
            {
                Success = true,
                HttpStatusCode = HttpStatusCode.Created
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
