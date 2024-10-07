using Application.UseCases.Base;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.UseCases.Motorcycle.Delete
{
    public class MotorcycleDeleteRequest : IRequest<CustomResponse>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }
}
