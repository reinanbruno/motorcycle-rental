using Application.UseCases.Base;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Rental.Create
{
    public class RentalCreateHandler : IRequestHandler<RentalCreateRequest, CustomResponse>, IDisposable
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IRentalPlanRepository _rentalPlanRepository;
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IDeliveryPersonRepository _deliveryPersonRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RentalCreateHandler(IRentalRepository rentalRepository, IRentalPlanRepository rentalPlanRepository, IMotorcycleRepository motorcycleRepository, IDeliveryPersonRepository deliveryPersonRepository, IUnitOfWork unitOfWork)
        {
            _rentalRepository = rentalRepository;
            _rentalPlanRepository = rentalPlanRepository;
            _motorcycleRepository = motorcycleRepository;
            _deliveryPersonRepository = deliveryPersonRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomResponse> Handle(RentalCreateRequest request, CancellationToken cancellationToken)
        {
            if (await _motorcycleRepository.GetAsync(x => x.Id == request.MotorcycleId, cancellationToken) is null)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Response = CustomResponseMessage.FromMessage("Não existe nenhuma moto com esse identificador."),
                };
            }

            Domain.Entities.DeliveryPerson deliveryPerson = await _deliveryPersonRepository.GetAsync(x => x.Id == request.DeliveryPersonId, cancellationToken);
            if (deliveryPerson is null)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Response = CustomResponseMessage.FromMessage("Não existe nenhum entregador com esse identificador."),
                };
            }

            if (deliveryPerson.DriverLicenseType != DriverLicenseType.A)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.BadGateway,
                    Response = CustomResponseMessage.FromMessage("Somente entregadores do tipo de categoria A pode fazer uma locação."),
                };
            }

            if (await _rentalRepository.GetAsync(x => x.DeliveryPersonId == request.DeliveryPersonId && x.ReturnDate == null, cancellationToken) is not null)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.Conflict,
                    Response = CustomResponseMessage.FromMessage("Esse entregador já tem uma locação, é necessário finalizar uma locação para solicitar outra."),
                };
            }

            Domain.Entities.RentalPlan rentalPlan = await _rentalPlanRepository.GetAsync(x => x.DurationDays == request.Plan, cancellationToken);
            if (rentalPlan is null)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Response = CustomResponseMessage.FromMessage("Não existe nenhum plano com essa quantidade de dias."),
                };
            }

            var entity = request.ToEntity(rentalPlan.Id);
            await _rentalRepository.CreateAsync(entity);
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
                HttpStatusCode = HttpStatusCode.Created,
                Response = new RentalCreateResponse
                {
                    Id = entity.Id
                }
            };
        }

        public void Dispose()
        {
            _motorcycleRepository?.Dispose();
            _deliveryPersonRepository?.Dispose();
            _rentalPlanRepository?.Dispose();
            _rentalRepository?.Dispose();
            _unitOfWork?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
