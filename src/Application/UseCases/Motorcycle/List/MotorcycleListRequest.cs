using MediatR;
using Application.UseCases.Base;
using System.Text.Json.Serialization;

namespace Application.UseCases.Motorcycle.List
{
    public class MotorcycleListRequest : IRequest<CustomResponse>
    {
        public string Plate { get; set; }
    }
}
