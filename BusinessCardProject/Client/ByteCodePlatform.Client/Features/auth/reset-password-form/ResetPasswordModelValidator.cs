using FluentValidation;

namespace BusinessCardProject.Client.Features.auth.reset_password_form
{
    public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
    {
        public ResetPasswordModelValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Пароль обязателен для заполнения.")
                .MinimumLength(6).WithMessage("Пароль должен содержать не менее 6 символов.")
                .Matches("[A-ZА-ЯЁ]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву.")
                .Matches("[a-zа-яё]").WithMessage("Пароль должен содержать хотя бы одну строчную букву.")
                .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру.")
                .Matches("[^a-zA-Z0-9а-яёА-ЯЁ0-9]")
                .WithMessage("Пароль должен содержать хотя бы один специальный символ (например, !@#$%^&*).")
                .Must((model, newPassword) => string.IsNullOrEmpty(model.ConfirmPassword) || newPassword == model.ConfirmPassword)
                .WithMessage("Пароли не совпадают.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Подтверждение пароля обязательно.")
                .Equal(x => x.NewPassword).WithMessage("Пароли не совпадают.");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var context = new ValidationContext<ResetPasswordModel>((ResetPasswordModel)model);
            var result = await ValidateAsync(context);

            if (result.IsValid)
            {
                return [];
            }
            
            return result.Errors
                .Where(e => e.PropertyName == propertyName)
                .Select(e => e.ErrorMessage);
        };
    }
}