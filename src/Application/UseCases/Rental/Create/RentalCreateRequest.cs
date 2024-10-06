using MediatR;
using Application.UseCases.Base;
using System.Text.Json.Serialization;

namespace Application.UseCases.Rental.Create
{
    public class RentalCreateRequest : IRequest<CustomResponse>
    {
        [JsonPropertyName("entregador_id")]
        public Guid DeliveryPersonId { get; set; }

        [JsonPropertyName("moto_id")]
        public Guid MotorcycleId { get; set; }

        [JsonPropertyName("plan")]
        public int Plan { get; set; }

        [JsonPropertyName("data_inicio")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("data_termino")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("data_previsao_termino")]
        public DateTime ExpectedEndDate { get; set; }

        public Domain.Entities.Rental ToEntity(Guid rentalPlanId)
        {
            return new Domain.Entities.Rental
            {
                Id = Guid.NewGuid(),
                DeliveryPersonId = this.DeliveryPersonId,
                MotorcycleId = this.MotorcycleId,
                RentalPlanId = rentalPlanId,
                StartDate = DateTime.SpecifyKind(this.StartDate, DateTimeKind.Unspecified),
                EndDate = DateTime.SpecifyKind(this.EndDate, DateTimeKind.Unspecified),
                ExpectedEndDate = DateTime.SpecifyKind(this.ExpectedEndDate, DateTimeKind.Unspecified)
            };
        }
    }
}
