using FluentValidation;

namespace BusinessCardProject.Client.Features.auth.login_form
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Некорректный формат Email");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(6).WithMessage("Минимум 6 символов");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var context = new ValidationContext<LoginModel>((LoginModel)model);
            var result = await ValidateAsync(context);
            if (result.IsValid)
                return [];

            return result.Errors
                .Where(e => e.PropertyName == propertyName || string.IsNullOrEmpty(propertyName))
                .Select(e => e.ErrorMessage);
        };
    }
}