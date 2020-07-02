namespace Assets.Scripts.Manage.Localization
{
    public class Word
    {
        public string Russian { get; set; }
        public string English { get; set; }

        public string Translate(Language language)
        {
            switch (language)
            {
                case Language.Russian:
                    return Russian;
                case Language.English:
                    return English;
                default:
                    return language.ToString();
            }
        }
    }
}