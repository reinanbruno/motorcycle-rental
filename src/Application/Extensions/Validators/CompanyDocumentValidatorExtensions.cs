using FluentValidation;

namespace Application.Extensions.Validators
{
    public static class CompanyDocumentValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidateCompanyDocument<T>(this IRuleBuilder<T, string> ruleBuilder, string propertyName)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{propertyName} é obrigatório.")
                .Must(ValidateCompanyDocument).WithMessage($"{propertyName} inválido! Deve conter apenas números sem máscara.");
        }

        private static bool ValidateCompanyDocument(string companyDocument)
        {
            companyDocument = new string(companyDocument.Where(char.IsDigit).ToArray());

            if (companyDocument.Length != 14)
                return false;

            int[] multiplicator1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicator2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempDocument = companyDocument.Substring(0, 12);
            int sum = 0;

            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempDocument[i].ToString()) * multiplicator1[i];

            int remainder = (sum % 11);
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;

            string digit = remainder.ToString();
            tempDocument += digit;
            sum = 0;

            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempDocument[i].ToString()) * multiplicator2[i];

            remainder = (sum % 11);
            if (remainder < 2)
                remainder = 0;
            else
                remainder = 11 - remainder;

            digit += remainder.ToString();

            return companyDocument.EndsWith(digit);
        }

    }
}
