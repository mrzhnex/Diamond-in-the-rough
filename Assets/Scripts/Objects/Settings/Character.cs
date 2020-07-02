namespace Assets.Scripts.Objects.Settings
{
    public class Character
    {
        public string Name { get; set; }
        public ulong Level { get; set; }
        public ulong Experience { get; set; }
        public Character(string Name, ulong Level, ulong Experience)
        {
            this.Name = Name;
            this.Level = Level;
            this.Experience = Experience;
        }
        protected Character() { }
    }
}