using FluentValidation;

namespace Application.UseCases.Motorcycle.Update
{
    public class MotorcycleUpdateValidator : AbstractValidator<MotorcycleUpdateRequest>
    {
        public MotorcycleUpdateValidator()
        {
            RuleFor(x => x.Plate)
                .NotEmpty().WithMessage("Placa é obrigatória.")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("A placa deve conter apenas letras e números.")
                .Length(7).WithMessage("Tamanho da placa tem que ter 7 caracteres.");
        }
    }
}
