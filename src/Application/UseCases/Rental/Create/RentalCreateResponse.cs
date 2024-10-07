using System.Text.Json.Serialization;

namespace Application.UseCases.Rental.Create
{
    public class RentalCreateResponse
    {
        [JsonPropertyName("identificador")]
        public Guid Id { get; set; }
    }
}
