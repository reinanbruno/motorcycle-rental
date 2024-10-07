using FluentValidation;

namespace Application.UseCases.Motorcycle.Create
{
    public class MotorcycleCreateValidator : AbstractValidator<MotorcycleCreateRequest>
    {
        public MotorcycleCreateValidator()
        {
            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Modelo é obrigatório.")
                .MaximumLength(50).WithMessage("Tamanho máximo do modelo é 50 caracteres.");

            RuleFor(x => x.Plate)
                .NotEmpty().WithMessage("Placa é obrigatória.")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("A placa deve conter apenas letras e números.")
                .Length(7).WithMessage("Tamanho da placa tem que ter 7 caracteres.");

            RuleFor(x => x.FabricationYear)
                .InclusiveBetween(1900, 2024).WithMessage("O ano deve ser entre 1900 e 2024.");

        }
    }
}
