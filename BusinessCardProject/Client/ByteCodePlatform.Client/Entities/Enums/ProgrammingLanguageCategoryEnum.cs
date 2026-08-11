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

        /// <summary>
        /// Для получения выбранного ЯП из настроек
        /// </summary>
        public static string GetNameFromSequenceNumber(int sequenceNumber)
        {
            var request = List().SingleOrDefault(sn => sn.SequenceNumber == sequenceNumber);
            if (request != null) return request.Name;
            {
                return Csharp.Name;
            }
        }

        private static readonly ProgrammingLanguageCategoryEnum Csharp = new(
            Guid.Parse("3df167d0-eb51-4b1f-a242-94f6638868fc"),
            "C#".ToUpperInvariant(),
            0);

        private static readonly ProgrammingLanguageCategoryEnum Php = new(
            Guid.Parse("39ccde21-dfbf-4203-a448-3fa132183445"),
            "PHP".ToUpperInvariant(),
            1);
    }
}