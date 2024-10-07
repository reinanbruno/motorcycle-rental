using Application.UseCases.Base;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.UseCases.DeliveryPerson.Update
{
    public class DeliveryPersonUpdateRequest : IRequest<CustomResponse>
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonPropertyName("imagem_cnh")]
        public string DriverLicenseImage { get; set; }
    }
}
