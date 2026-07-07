using System.Text.Json.Serialization;

namespace BusinessCardProject.Client.Entities.Services.ProjectInfo.Options
{
    internal class UserLanguagesList
    {
        private int _selectedProgrammingLanguage;

        [JsonInclude] public int ProgrammingLanguage => _selectedProgrammingLanguage;

        [JsonConstructor]
        public UserLanguagesList(int programmingLanguage = 0)
        {
            _selectedProgrammingLanguage = programmingLanguage;
        }

        public UserLanguagesList()
        {
        }

        public void SetProgrammingLanguage(int programmingLanguage)
        {
            _selectedProgrammingLanguage = programmingLanguage;
        }
    }
}