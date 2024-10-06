using FluentValidation;
using Application.Extensions.Validators;

namespace Application.UseCases.DeliveryPerson.Create
{
    public class DeliveryPersonCreateValidator : AbstractValidator<DeliveryPersonCreateRequest>
    {
        public DeliveryPersonCreateValidator()
        {
            RuleFor(x => x.DriverLicenseNumber)
                .NotEmpty().WithMessage("Número da CNH é obrigatório.")
                .Length(11).WithMessage("Tamanho do número da CNH tem que ter 11 caracteres.");

            RuleFor(x => x.DriverLicenseType)
                .IsInEnum().WithMessage("Tipo de CNH inválido.");
            
            RuleFor(x => x.DriverLicenseImage)
                .ValidateBase64Image("Imagem da CNH");

            RuleFor(x => x.DateOfBirth)
                .InclusiveBetween(DateTime.Now.AddYears(-100), DateTime.Now).WithMessage($"A data de nascimento deve ser entre {DateTime.Now.AddYears(-100).ToShortDateString()} e {DateTime.Now.ToShortDateString()}.");

            RuleFor(x => x.Document)
                .ValidateCompanyDocument("CNPJ");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .Matches("^[a-zA-Z0-9]*$").WithMessage("O nome deve conter apenas letras e números.")
                .MaximumLength(100).WithMessage("Tamanho máximo do nome é 100 caracteres.");
        }
    }
}
