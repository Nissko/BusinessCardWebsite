using System.Text.Json.Serialization;

namespace BusinessCardProject.Client.Entities.Services.ProjectInfo.Options
{
    internal class UserLanguagesList
    {
        [JsonInclude] public int SelectedProgrammingLanguage { get; private set; }

        [JsonConstructor]
        public UserLanguagesList(int programmingLanguage = 0)
        {
            SelectedProgrammingLanguage = programmingLanguage;
        }

        public UserLanguagesList()
        {
        }

        public void SetProgrammingLanguage(int programmingLanguage)
        {
            SelectedProgrammingLanguage = programmingLanguage;
        }
    }
}