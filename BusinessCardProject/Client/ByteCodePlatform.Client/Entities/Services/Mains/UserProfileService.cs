using AuthorizationService.Proto;

namespace BusinessCardProject.Client.Entities.Services.Mains
{
    public class UserProfileService : IDisposable
    {
        private UserInfoResponse? _cachedUser;
        public event Func<Task>? OnUserProfileChanged;

        /// <summary>
        /// Информация о добавленном пользователе
        /// </summary>
        public UserInfoResponse? UserInCache => _cachedUser;

        /// <summary>
        /// Добавить информацию о пользователе в кэш
        /// </summary>
        public void SetUserInCache(UserInfoResponse user) 
        {
            Interlocked.Exchange(ref _cachedUser, user);
        }

        public async Task UpdateUserProfileEvent()
        {
            var handler = OnUserProfileChanged;
            if (handler is null) return;
        
            foreach (var subscribers in handler.GetInvocationList())
            {
                try
                {
                    await ((Func<Task>)subscribers).Invoke();
                }
                catch
                {
                    // ignored
                }
            }
        }

        public void Dispose()
        {
            OnUserProfileChanged = null;
            GC.SuppressFinalize(this);
        }
    }
}