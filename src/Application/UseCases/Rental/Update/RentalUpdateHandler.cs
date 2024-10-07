using Application.UseCases.Base;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Rental.Update
{
    public class RentalUpdateHandler : IRequestHandler<RentalUpdateRequest, CustomResponse>, IDisposable
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RentalUpdateHandler(IRentalRepository rentalRepository, IUnitOfWork unitOfWork)
        {
            _rentalRepository = rentalRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomResponse> Handle(RentalUpdateRequest request, CancellationToken cancellationToken)
        {
            Domain.Entities.Rental entity = await _rentalRepository.GetAsync(x => x.Id == request.Id, cancellationToken, i => i.RentalPlan);
            if (entity is null)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Response = CustomResponseMessage.FromMessage("Identificador não localizado."),
                };
            }

            if (entity.ReturnDate is not null)
            {
                return new CustomResponse
                {
                    Success = false,
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Response = CustomResponseMessage.FromMessage("Locação não disponível para devolução."),
                };
            }

            entity.ReturnDate = DateTime.SpecifyKind(request.ReturnDate, DateTimeKind.Unspecified);
            entity.TotalAmount = entity.CalculateTotalAmount(request.ReturnDate);
            _rentalRepository.Update(entity);
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
            _rentalRepository?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
