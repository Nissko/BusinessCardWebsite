namespace BusinessCardProject.Client.Services.ProjectInfoService.Classes;

/// <summary>
/// Получение серверных изображений в зависимости от темы
/// </summary>
public class ImageByThemeClass
{
    private bool _isDark;
    
    /// <summary>
    /// Получение серверных изображений в зависимости от темы
    /// </summary>
    public ImageByThemeClass(bool theme)
    {
        _isDark = theme;
    }
    
    /// <summary>
    /// QR для пожертвований
    /// </summary>
    public string DonationQrCodeImage => _isDark ? "images/Donation/QR_Light.png" : "images/Donation/QR_Dark.png";
}