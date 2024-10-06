using MediatR;
using Application.UseCases.Base;
using Application.UseCases.Rental.Get;
using Domain.Repositories;
using System.Net;

namespace Application.UseCases.Rental.List
{
    public class RentalGetHandler : IRequestHandler<RentalGetRequest, CustomResponse>, IDisposable
    {
        private readonly IRentalRepository _rentalRepository;

        public RentalGetHandler(IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
        }

        public async Task<CustomResponse> Handle(RentalGetRequest request, CancellationToken cancellationToken)
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

            return new CustomResponse
            {
                Success = true,
                HttpStatusCode = HttpStatusCode.OK,
                Response = RentalGetResponse.FromEntity(entity)
            };
        }

        public void Dispose()
        {
            _rentalRepository?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
