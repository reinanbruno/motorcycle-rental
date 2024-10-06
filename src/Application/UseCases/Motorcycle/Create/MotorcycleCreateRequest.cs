using MediatR;
using Application.UseCases.Base;
using System.Text.Json.Serialization;

namespace Application.UseCases.Motorcycle.Create
{
    public class MotorcycleCreateRequest : IRequest<CustomResponse>
    {
        [JsonPropertyName("identificador")]
        public Guid Id { get; set; }

        [JsonPropertyName("ano")]
        public int FabricationYear { get; set; }
        
        [JsonPropertyName("modelo")]
        public string Model { get; set; }

        [JsonPropertyName("placa")]
        public string Plate { get; set; }

        public Domain.Entities.Motorcycle ToEntity()
        {
            return new Domain.Entities.Motorcycle
            {
                Id = this.Id,
                FabricationYear = this.FabricationYear,
                Model = this.Model,
                Plate = this.Plate
            };
        }
    }
}
