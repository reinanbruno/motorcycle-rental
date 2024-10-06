using System.Text.Json.Serialization;

namespace Application.UseCases.Rental.Get
{
    public class RentalGetResponse
    {
        [JsonPropertyName("identificador")]
        public Guid Id { get; set; }

        [JsonPropertyName("valor_diaria")]
        public decimal DailyAmount { get; set; }

        [JsonPropertyName("entregador_id")]
        public Guid DeliveryPersonId { get; set; }

        [JsonPropertyName("moto_id")]
        public Guid MotorcycleId { get; set; }

        [JsonPropertyName("data_inicio")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("data_termino")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("data_previsao_termino")]
        public DateTime ExpectedEndDate { get; set; }
        
        [JsonPropertyName("data_devolucao")]
        public DateTime? ReturnDate { get; set; }

        [JsonPropertyName("valor_total")]
        public decimal? TotalAmount { get; set; }

        public static RentalGetResponse FromEntity(Domain.Entities.Rental entity)
        {
            return new RentalGetResponse
            {
                Id = entity.Id,
                DeliveryPersonId = entity.DeliveryPersonId,
                MotorcycleId = entity.MotorcycleId,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                ExpectedEndDate = entity.ExpectedEndDate,
                ReturnDate = entity.ReturnDate,
                TotalAmount = entity.TotalAmount,
                DailyAmount = entity.RentalPlan.DailyAmount
            };
        }
    }
}
