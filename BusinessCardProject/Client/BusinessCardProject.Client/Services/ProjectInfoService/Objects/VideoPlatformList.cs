using System.Text.Json.Serialization;

namespace BusinessCardProject.Client.Services.ProjectInfoService.Objects;

public class VideoPlatformList
{
    private bool _youTube;
    private bool _ruTube;
    private bool _vkVideo;

    [JsonInclude]
    public bool YouTube => _youTube;
    [JsonInclude]
    public bool RuTube => _ruTube;
    [JsonInclude]
    public bool VkVideo => _vkVideo;

    // Конструктор для десериализации
    [JsonConstructor]
    public VideoPlatformList(bool youTube = false, bool ruTube = false, bool vkVideo = false)
    {
        _youTube = youTube;
        _ruTube = ruTube;
        _vkVideo = vkVideo;
    }

    public VideoPlatformList() { }

    public void SetYouTube()
    {
        _youTube = true;
        _ruTube = false;
        _vkVideo = false;
    }

    public void SetRuTube()
    {
        _youTube = false;
        _ruTube = true;
        _vkVideo = false;
    }

    public void SetVkVideo()
    {
        _youTube = false;
        _ruTube = false;
        _vkVideo = true;
    }
}