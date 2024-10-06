namespace Domain.Entities
{
    public class RentalPlan : BaseEntity
    {
        public int DurationDays { get; set; }
        public decimal DailyAmount { get; set; }
        public decimal FinePercentage { get; set; }
    }
}
