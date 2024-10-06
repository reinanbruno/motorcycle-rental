using FluentValidation;

namespace Application.UseCases.Rental.Update
{
    public class RentalUpdateValidator : AbstractValidator<RentalUpdateRequest>
    {
        public RentalUpdateValidator()
        {
            RuleFor(x => x.ReturnDate)
               .GreaterThanOrEqualTo(DateTime.Now).WithMessage("A data de devolução tem que ser maior ou igual a data de hoje.");
        }
    }
}
