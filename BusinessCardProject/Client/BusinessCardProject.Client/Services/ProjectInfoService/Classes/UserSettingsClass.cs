using BusinessCardProject.Client.Services.ProjectInfoService.Objects;

namespace BusinessCardProject.Client.Services.ProjectInfoService.Classes;

public class UserSettingsClass
{

    /// <summary>
    /// Тема
    /// </summary>
    public bool IsDark { get; set; }
    /// <summary>
    /// Открытие меню
    /// TODO:Пересмотреть реализацию
    /// <remarks>Можно не использовать?</remarks>
    /// </summary>
    public bool IsDrawerOpen { get; set; }
    public string Language { get; set; } = "ru";

    /// <summary>
    /// Дата получения
    /// TODO: Сделать реализацию. Что если Null, то должны делать Load(), а затем заполнять UpdateTime
    /// </summary>
    public DateTime? UpdateTime { get; set; } = null;

    public VideoPlatformList? VideoPlatform { get; set; } = null;
}