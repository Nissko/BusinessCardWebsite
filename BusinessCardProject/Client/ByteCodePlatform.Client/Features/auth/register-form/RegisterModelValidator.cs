using FluentValidation;

namespace BusinessCardProject.Client.Features.auth.register_form
{
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(x => x.Surname)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Фамилия обязательна")
                .MinimumLength(2).WithMessage("Минимум 2 символа");

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Имя обязательно")
                .MinimumLength(2).WithMessage("Минимум 2 символа");

            RuleFor(x => x.NickName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Никнейм обязателен")
                .MinimumLength(3).WithMessage("Минимум 3 символа");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Некорректный формат Email");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(6).WithMessage("Пароль должен содержать не менее 6 символов.")
                .Matches("[A-ZА-ЯЁ]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву.")
                .Matches("[a-zа-яё]").WithMessage("Пароль должен содержать хотя бы одну строчную букву.")
                .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру.")
                .Matches("[^a-zA-Z0-9а-яёА-ЯЁ0-9]")
                .WithMessage("Пароль должен содержать хотя бы один специальный символ (например, !@#$%^&*).");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var context = new ValidationContext<RegisterModel>((RegisterModel)model);
            var result = await ValidateAsync(context);
            if (result.IsValid)
                return [];

            return result.Errors
                .Where(e => e.PropertyName == propertyName || string.IsNullOrEmpty(propertyName))
                .Select(e => e.ErrorMessage);
        };
    }
}