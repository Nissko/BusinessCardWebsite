namespace BusinessCardProject.Client.Features.auth.reset_password_form
{
    public class ResetPasswordModel
    {
        /// <summary>
        /// Новый пароль
        /// </summary>
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Пароль для подтверждения
        /// </summary>
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}