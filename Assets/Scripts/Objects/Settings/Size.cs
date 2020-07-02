using System;

namespace Assets.Scripts.Objects.Settings
{
    [Serializable]
    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }

        protected Size() { }
        public Size(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }
        public override string ToString()
        {
            return nameof(Width) + ": " + Width + " " + nameof(Height) + ": " + Height;
        }
    }
}