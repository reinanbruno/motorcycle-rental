namespace Domain.Entities
{
    public class Motorcycle : BaseEntity
    {
        public int FabricationYear { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }
    }
}
