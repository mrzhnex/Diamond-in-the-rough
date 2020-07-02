using System;
using UnityEngine;

namespace Assets.Scripts.Objects.Settings
{
    [Serializable]
    public class MovementBind
    {
        public KeyCode Left;
        public KeyCode Right;
        public KeyCode Up;
        public KeyCode Down;
        public KeyCode Swift;
        protected MovementBind() { }
        public MovementBind(KeyCode Left, KeyCode Right, KeyCode Up, KeyCode Down, KeyCode Swift)
        {
            this.Left = Left;
            this.Right = Right;
            this.Up = Up;
            this.Down = Down;
            this.Swift = Swift;
        }
    }
}