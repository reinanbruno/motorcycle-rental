using MediatR;
using Application.UseCases.Base;
using System.Text.Json.Serialization;

namespace Application.UseCases.Motorcycle.Update
{
    public class MotorcycleUpdateRequest : IRequest<CustomResponse>
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonPropertyName("placa")]
        public string Plate { get; set; }
    }
}
