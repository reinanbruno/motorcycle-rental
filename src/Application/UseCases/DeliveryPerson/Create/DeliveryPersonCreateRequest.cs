using Application.UseCases.Base;
using Domain.Enums;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.UseCases.DeliveryPerson.Create
{
    public class DeliveryPersonCreateRequest : IRequest<CustomResponse>
    {
        [JsonPropertyName("identificador")]
        public Guid Id { get; set; }

        [JsonPropertyName("nome")]
        public string Name { get; set; }

        [JsonPropertyName("cnpj")]
        public string Document { get; set; }

        [JsonPropertyName("data_nascimento")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("numero_cnh")]
        public string DriverLicenseNumber { get; set; }

        [JsonPropertyName("tipo_cnh")]
        public DriverLicenseType DriverLicenseType { get; set; }

        [JsonPropertyName("imagem_cnh")]
        public string DriverLicenseImage { get; set; }

        public Domain.Entities.DeliveryPerson ToEntity(string driverLicenseImage)
        {
            return new Domain.Entities.DeliveryPerson
            {
                Id = this.Id,
                Name = this.Name,
                Document = this.Document,
                DateOfBirth = DateTime.SpecifyKind(this.DateOfBirth, DateTimeKind.Unspecified),
                DriverLicenseNumber = this.DriverLicenseNumber,
                DriverLicenseType = this.DriverLicenseType,
                DriverLicenseImage = driverLicenseImage
            };
        }
    }
}
