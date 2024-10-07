using Application.Extensions.Validators;
using FluentValidation;

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
