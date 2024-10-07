using Application.UseCases.Base;
using MediatR;

namespace Application.UseCases.Motorcycle.List
{
    public class MotorcycleListRequest : IRequest<CustomResponse>
    {
        public string Plate { get; set; }
    }
}
