using ByteCodePlatform.Domain.Common;

namespace ByteCodePlatform.Domain.Enums
{
    public class ProgrammingLanguageCategoryEnum : Enumeration
    {
        public ProgrammingLanguageCategoryEnum(Guid id, string name) : base(id, name)
        { }

        public static IEnumerable<ProgrammingLanguageCategoryEnum> List()
        {
            return
            [
                Csharp,
                Php
            ];
        }

        public static ProgrammingLanguageCategoryEnum FromName(string typeOfCourseFromName)
        {
            var request = List()
                .SingleOrDefault(s =>
                    string.Equals(s.Name, typeOfCourseFromName, StringComparison.CurrentCultureIgnoreCase));

            if (request != null) return request;
            {
                var typeOfCourseIsExists = string.Join(",", List().Select(s => s.Name));
                throw new ArgumentNullException(typeOfCourseIsExists);
            }
        }
    
        public static ProgrammingLanguageCategoryEnum FromId(Guid fieldTypeId)
        {
            var request = List().SingleOrDefault(s => s.Id == fieldTypeId);

            if (request != null) return request;
            {
                var typeOfCourseIsExists = string.Join(",", List().Select(s => s.Id));

                throw new(typeOfCourseIsExists);
            }
        }

        public static readonly ProgrammingLanguageCategoryEnum Csharp = new(
            Guid.Parse("3df167d0-eb51-4b1f-a242-94f6638868fc"),
            "C#".ToUpperInvariant());

        public static readonly ProgrammingLanguageCategoryEnum Php = new(
            Guid.Parse("39ccde21-dfbf-4203-a448-3fa132183445"),
            "PHP".ToUpperInvariant());
    }
}