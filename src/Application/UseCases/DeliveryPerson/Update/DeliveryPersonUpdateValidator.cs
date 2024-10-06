using FluentValidation;
using Application.Extensions.Validators;

namespace Application.UseCases.DeliveryPerson.Update
{
    public class DeliveryPersonUpdateValidator : AbstractValidator<DeliveryPersonUpdateRequest>
    {
        public DeliveryPersonUpdateValidator()
        {
            RuleFor(x => x.DriverLicenseImage)
                .ValidateBase64Image("Imagem da CNH");
        }
    }
}
