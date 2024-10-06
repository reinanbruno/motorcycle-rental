using MediatR;
using Application.UseCases.Base;
using System.Text.Json.Serialization;

namespace Application.UseCases.Rental.Update
{
    public class RentalUpdateRequest : IRequest<CustomResponse>
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonPropertyName("data_devolucao")]
        public DateTime ReturnDate { get; set; }
    }
}
