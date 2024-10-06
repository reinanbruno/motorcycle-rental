using MediatR;
using Application.UseCases.Base;
using System.Text.Json.Serialization;

namespace Application.UseCases.Motorcycle.Delete
{
    public class MotorcycleDeleteRequest : IRequest<CustomResponse>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }
}
