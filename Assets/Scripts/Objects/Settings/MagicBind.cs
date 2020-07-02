using System;
using UnityEngine;

namespace Assets.Scripts.Objects.Settings
{
    [Serializable]
    public class MagicBind
    {
        public KeyCode Power;
        public KeyCode Speed;
        public KeyCode Area;
        public KeyCode Stability;

        protected MagicBind() { }

        public MagicBind(KeyCode Power, KeyCode Speed, KeyCode Area, KeyCode Stability)
        {
            this.Power = Power;
            this.Speed = Speed;
            this.Area = Area;
            this.Stability = Stability;
        }
    }
}