using Application.UseCases.Base;
using MediatR;
using System.Text.Json;

namespace Application.UseCases.Motorcycle.Events.Created
{
    public class MotorcycleCreatedEventRequest : IRequest<CustomResponse>
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public int FabricationYear { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }

        public Domain.Entities.Notification ToEntity()
        {
            return new Domain.Entities.Notification
            {
                Id = Guid.NewGuid(),
                EventType = nameof(MotorcycleCreatedEventRequest),
                Message = JsonSerializer.Serialize(this)
            };
        }
    }
}
