using MediatR;
using Application.UseCases.Base;
using System.Text.Json.Serialization;

namespace Application.UseCases.Motorcycle.Get
{
    public class MotorcycleGetRequest : IRequest<CustomResponse>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }
}
