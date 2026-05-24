using Services.AuthService.Domain.Common;

namespace Services.AuthService.Domain.Enums
{
    public class UserRoleEnum : Enumeration
    {
        private UserRoleEnum(Guid id, string name) : base(id, name)
        { }

        public static IEnumerable<UserRoleEnum> List()
        {
            return
            [
                User,
                Admin,
                Author
            ];
        }

        /// <summary>
        /// Получение статуса пользователя по названию
        /// </summary>
        public static UserRoleEnum FromName(string typeOfCourseFromName)
        {
            var request = List()
                .SingleOrDefault(s =>
                    string.Equals(s.Name, typeOfCourseFromName, StringComparison.CurrentCultureIgnoreCase));

            if (request != null) return request;
            {
                var typeOfCourseIsExists = string.Join(",", List().Select(s => s.Name));

                /*TODO: Кастомное исключение*/
                throw new ArgumentNullException(typeOfCourseIsExists);
            }
        }

        /// <summary>
        /// Получение статуса пользователя по его Id
        /// </summary>
        public static UserRoleEnum FromId(Guid fieldTypeId)
        {
            var request = List().SingleOrDefault(s => s.Id == fieldTypeId);

            if (request != null) return request;
            {
                var typeOfCourseIsExists = string.Join(",", List().Select(s => s.Id));

                throw new(typeOfCourseIsExists);
            }
        }
    
        public static readonly UserRoleEnum User = new(
            Guid.Parse("c770f398-31bc-4800-9aae-c07892b74702"),
            "Пользователь".ToLowerInvariant());
    
        public static readonly UserRoleEnum Admin = new(
            Guid.Parse("c2b7f59c-df80-4a13-a993-a4b0c893181a"),
            "Администратор".ToLowerInvariant());
        
        public static readonly UserRoleEnum Author = new(
            Guid.Parse("287573cd-6893-4944-a100-83ade2f4b7bf"),
            "Автор".ToLowerInvariant());
    }
}