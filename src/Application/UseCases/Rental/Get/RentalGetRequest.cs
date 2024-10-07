using Application.UseCases.Base;
using MediatR;

namespace Application.UseCases.Rental.List
{
    public class RentalGetRequest : IRequest<CustomResponse>
    {
        public Guid Id { get; set; }
    }
}
