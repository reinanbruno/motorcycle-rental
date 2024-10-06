using System.Text.Json.Serialization;

namespace Application.UseCases.Motorcycle.Get
{
    public class MotorcycleGetResponse
    {
        [JsonPropertyName("identificador")]
        public Guid Id { get; set; }

        [JsonPropertyName("ano")]
        public int FabricationYear { get; set; }

        [JsonPropertyName("modelo")]
        public string Model { get; set; }

        [JsonPropertyName("placa")]
        public string Plate { get; set; }

        [JsonPropertyName("data_criacao")]
        public DateTime CreationDate { get; set; }

        public static MotorcycleGetResponse FromEntity(Domain.Entities.Motorcycle entity)
        {
            return new MotorcycleGetResponse
            {
                Id = entity.Id,
                FabricationYear = entity.FabricationYear,
                Model = entity.Model,
                Plate = entity.Plate,
                CreationDate = entity.CreationDate
            };
        }
    }
}
