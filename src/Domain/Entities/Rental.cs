namespace Domain.Entities
{
    public class Rental : BaseEntity
    {
        public Guid DeliveryPersonId { get; set; }
        public Guid MotorcycleId { get; set; }
        public Guid RentalPlanId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public RentalPlan RentalPlan { get; set; }
        public DeliveryPerson DeliveryPerson { get; set; }
        public Motorcycle Motorcycle { get; set; }

        public decimal CalculateTotalAmount(DateTime returnDate)
        {

            var usedDays = (returnDate - StartDate).Days;
            var amountUsedDays = usedDays * RentalPlan.DailyAmount;
            if (returnDate < ExpectedEndDate)
            {
                var unusedDays = (ExpectedEndDate - returnDate).Days;
                var amountUnusedDays = (unusedDays * RentalPlan.DailyAmount);
                var fineAmount = (amountUnusedDays * RentalPlan.FinePercentage) / 100;
                return amountUsedDays + amountUnusedDays + fineAmount;
            }

            if (returnDate > ExpectedEndDate)
            {
                var extraDays = (returnDate - ExpectedEndDate).Days;
                var additionalCost = extraDays * 50;
                return amountUsedDays + additionalCost;
            }

            return amountUsedDays;
        }
    }
}
