using AuthorizationService.Proto;
using BusinessCardProject.Client.Entities.Services.Mains;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace BusinessCardProject.Client.Widgets.user_profile.profile_header.avatar_upload_dialog
{
    public partial class AvatarUploadDialog : ComponentBase
    {
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
        [Parameter] public string? CurrentAvatarUrl { get; set; }
        
        [Inject] private UserProfileService UserProfileService { get; set; } = null!;
        [Inject] private FileUploadService FileUploadService { get; set; } = null!;
        [Inject] public AuthorizationService.Proto.AuthorizationService.AuthorizationServiceClient AuthorizationService { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;

        private IBrowserFile? SelectedFile { get; set; }
        private string? PreviewBase64 { get; set; }
        private string? ErrorMessage { get; set; }

        private const long MaxFileSize = 3 * 1024 * 1024;
        private static readonly HashSet<string> AllowedContentTypes = ["image/jpeg", "image/png", "image/webp"];
        private bool CanSubmit => SelectedFile is not null && string.IsNullOrEmpty(ErrorMessage);

        private async Task OnFileSelected(IBrowserFile file)
        {
            ErrorMessage = null;
            PreviewBase64 = null;
            SelectedFile = null;

            if (file.Size > MaxFileSize)
            {
                ErrorMessage = $"Файл слишком большой. Максимум {MaxFileSize / (1024 * 1024)} МБ.";
                return;
            }

            if (!AllowedContentTypes.Contains(file.ContentType))
            {
                ErrorMessage = "Допустимые форматы: JPEG, PNG, WebP.";
                return;
            }

            SelectedFile = file;

            try
            {
                await using var stream = file.OpenReadStream(MaxFileSize);
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                PreviewBase64 = $"data:{file.ContentType};base64,{Convert.ToBase64String(ms.ToArray())}";
            }
            catch
            {
                ErrorMessage = "Не удалось прочитать файл.";
                SelectedFile = null;
            }
        }

        private async Task ConfirmDialog()
        {
            if (SelectedFile is { Size: > 0 })
            { 
                var fileUploadValue = await FileUploadService.UploadImageAsync(SelectedFile);
                var result = await AuthorizationService.UpdateUserAvatarAsync(new UpdateUserAvatarRequest
                {
                    AvatarId = fileUploadValue
                });
                
                await UserProfileService.UpdateUserProfileEvent();
                
                if (!result.Result)
                {
                    Snackbar.Add("Произошла ошибка при изменении изображения", Severity.Warning);
                }
                else
                { 
                    Snackbar.Add("Изображение изменено", Severity.Success);
                }
                
                MudDialog.Close(DialogResult.Ok(SelectedFile));
            }
            else
            {
                MudDialog.Close(DialogResult.Cancel());
            }
        }

        private void CloseDialog()
        {
            MudDialog.Cancel();
        }

        private static string FormatFileSize(long bytes) => bytes switch
        {
            < 1024 => $"{bytes} Б",
            < 1024 * 1024 => $"{bytes / 1024.0:F1} КБ",
            _ => $"{bytes / (1024.0 * 1024.0):F1} МБ"
        };
    }
}