using FluentValidation;

namespace Application.UseCases.Rental.Create
{
    public class RentalCreateValidator : AbstractValidator<RentalCreateRequest>
    {
        public RentalCreateValidator()
        {
            RuleFor(x => x.Plan)
                .GreaterThan(0).WithMessage($"O plano tem que ser maior que 0.");

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTime.Now).WithMessage($"A data de inicio da locação tem que ser maior que {DateTime.Now.ToShortDateString()}.");

            RuleFor(x => x.EndDate)
               .GreaterThanOrEqualTo(x => x.StartDate.AddDays(x.Plan)).WithMessage("A data de término tem que ser maior ou igual a data de inicio + dias de plano.");

            RuleFor(x => x.ExpectedEndDate)
                .GreaterThanOrEqualTo(x => x.StartDate.AddDays(x.Plan)).WithMessage("A previsão de data de término tem que ser maior ou igual a data de inicio + dias de plano.");
        }
    }
}
