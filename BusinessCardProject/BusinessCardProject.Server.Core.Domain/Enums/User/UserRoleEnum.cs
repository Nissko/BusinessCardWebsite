using BusinessCardProject.Server.Core.Domain.Commons;

namespace BusinessCardProject.Server.Core.Domain.Enums.User
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
                Author,
                Admin
            ];
        }

        /// <summary>
        /// Получение типа пользователя по названию
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
        /// Получение типа пользователя по его Id
        /// </summary>
        public static UserRoleEnum FromId(Guid fieldTypeId)
        {
            var request = List().SingleOrDefault(s => s.Id == fieldTypeId);

            if (request != null) return request;
            {
                var typeOfCourseIsExists = string.Join(",", List().Select(s => s.Id));

                throw new Exception(typeOfCourseIsExists);
            }
        }
    
        public static readonly UserRoleEnum User = new(
            Guid.Parse("9a460a7b-600f-4662-a598-2e61cd64d171"),
            "Пользователь".ToLowerInvariant());
    
        public static readonly UserRoleEnum Author = new(
            Guid.Parse("4956dc45-e3eb-4944-ae14-41d591f31869"),
            "Автор".ToLowerInvariant());
    
        public static readonly UserRoleEnum Admin = new(
            Guid.Parse("48240e89-0d8d-4e4f-a6df-9776df45794c"),
            "Администратор".ToLowerInvariant());
    }
}