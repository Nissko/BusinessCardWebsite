namespace BusinessCardProject.Server.Core.Domain.Aggregates.User.Setting
{
    /// <summary>
    /// Настройки пользователя
    /// </summary>
    internal class UserSetting : Dictionary<string, object?>
    {
        public T? Get<T>(string key)
        {
            if (TryGetValue(key, out var value) && value is T typedValue)
                return typedValue;
            return default;
        }

        public void Set(string key, object? value)
        {
            this[key] = value;
        }
    }
}