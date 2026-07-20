namespace Services.MailService.Infrastructure.Settings
{
    public class MailSettings
    {
        public string DisplayName { get; set; } = string.Empty;
        public string From { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 465;
        public bool UseSsl { get; set; } = true;
    }
}