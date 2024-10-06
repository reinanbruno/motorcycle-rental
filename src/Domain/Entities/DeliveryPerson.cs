using Domain.Enums;

namespace Domain.Entities
{
    public class DeliveryPerson : BaseEntity
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string DriverLicenseNumber { get; set; }
        public DriverLicenseType DriverLicenseType { get; set; }
        public string DriverLicenseImage { get; set; }
    }
}
