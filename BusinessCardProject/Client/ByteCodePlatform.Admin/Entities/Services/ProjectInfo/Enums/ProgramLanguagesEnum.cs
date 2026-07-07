namespace ByteCodePlatform.Admin.Entities.Services.ProjectInfo.Enums
{
    internal class ProgramLanguagesEnum : Enumeration
    {
        public ProgramLanguagesEnum(Guid id, string name) : base(id, name)
        { }

        public static IEnumerable<ProgramLanguagesEnum> List()
        {
            return
            [
                Csharp,
                Php
            ];
        }

        public static ProgramLanguagesEnum FromName(string typeOfCourseFromName)
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
    
        public static ProgramLanguagesEnum FromId(Guid fieldTypeId)
        {
            var request = List().SingleOrDefault(s => s.Id == fieldTypeId);

            if (request != null) return request;
            {
                var typeOfCourseIsExists = string.Join(",", List().Select(s => s.Id));

                throw new(typeOfCourseIsExists);
            }
        }

        /*TODO: сделать везде ID одинаковые*/
        public static readonly ProgramLanguagesEnum Csharp = new(
            Guid.Parse("40e6391f-6e0b-4743-9893-38dbd30cfab2"),
            "C#".ToUpperInvariant());

        public static readonly ProgramLanguagesEnum Php = new(
            Guid.Parse("ef7a649a-0bc6-4b53-b9f5-f603587f540a"),
            "PHP".ToUpperInvariant());
    }
}