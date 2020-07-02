using System;

namespace Assets.Scripts.Objects.Settings
{
    [Serializable]
    public class KeyBind
    {
        public MagicBind MagicBind;
        public MovementBind MovementBind;
        public OtherBind OtherBind;
        public KeyBind(MagicBind MagicBind, MovementBind MovementBind, OtherBind OtherBind)
        {
            this.MagicBind = MagicBind;
            this.MovementBind = MovementBind;
            this.OtherBind = OtherBind;
        }
        protected KeyBind() { }
    }
}