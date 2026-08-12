namespace BusinessCardProject.Client.Entities.Enums
{
    internal class ProgrammingLanguageCategoryEnum : Enumeration
    {
        private ProgrammingLanguageCategoryEnum(Guid id, string name, int sequenceNum) : base(id, name, sequenceNum)
        {
        }

        private static IEnumerable<ProgrammingLanguageCategoryEnum> List()
        {
            return
            [
                Csharp,
                Php
            ];
        }

        // Добавили публичный метод для получения всех тем в UI
        public static IEnumerable<ProgrammingLanguageCategoryEnum> GetAll() => List();

        private static readonly ProgrammingLanguageCategoryEnum Csharp = new(
            Guid.Parse("40e6391f-6e0b-4743-9893-38dbd30cfab2"),
            "C#".ToUpperInvariant(),
            0);

        private static readonly ProgrammingLanguageCategoryEnum Php = new(
            Guid.Parse("ef7a649a-0bc6-4b53-b9f5-f603587f540a"),
            "PHP".ToUpperInvariant(),
            1);
    }
}