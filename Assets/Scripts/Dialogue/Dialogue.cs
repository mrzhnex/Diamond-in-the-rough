using UnityEngine;

namespace Assets.Scripts.Dialogue
{
    public delegate void DynamicFunc();
    public class Dialogue
    {
        public string AuthorMessage { get; set; }
        public Sprite AuthorSprite { get; set; }

        public DynamicFunc SubmitAction { get; set; }
        public DynamicFunc CancelAction { get; set; }
    }
}