using FluentValidation;

namespace Application.Extensions.Validators
{
    public static class Base64ImageValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidateBase64Image<T>(this IRuleBuilder<T, string> ruleBuilder, string propertyName)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{propertyName} é obrigatória.")
                .Must(ValidateBase64Image).WithMessage($"{propertyName} inválida.");
        }

        private static bool ValidateBase64Image(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                return false;

            string[] extensionsValids = ["png", "bmp"];
            return extensionsValids.Any(e => base64String.StartsWith($"data:image/{e};base64,"));
        }
    }
}
