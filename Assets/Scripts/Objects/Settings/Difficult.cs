namespace Assets.Scripts.Objects.Settings
{
    public class Difficult
    {
        public string DifficultName { get; set; }
        protected Difficult() { }
        public Difficult(string name = "Low Level")
        {
            DifficultName = name;
        }
    }
}