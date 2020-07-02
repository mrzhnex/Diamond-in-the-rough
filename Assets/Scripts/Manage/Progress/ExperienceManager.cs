namespace Assets.Scripts.Manage.Progress
{
    public static class ExperienceManager
    {
        public static readonly ulong ExperiencePerLevel = 10;
        public static ulong ExperienceByLevel(ulong level)
        {
            return level * ExperiencePerLevel * (level * level / ExperiencePerLevel);
        }


    }
}