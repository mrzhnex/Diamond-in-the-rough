using System;
using UnityEngine;

namespace Assets.Scripts.Objects.Settings
{
    [Serializable]
    public class OtherBind
    {
        public KeyCode DeveloperConsoleToggle;
        public KeyCode MenuToggle;
        public KeyCode DialogueSubmit;
        public KeyCode DialogueCancel;
        public KeyCode Respawn;
        protected OtherBind() { }
        public OtherBind(KeyCode DeveloperConsoleToggle, KeyCode MenuToggle, KeyCode DialogueSubmit, KeyCode DialogueCancel, KeyCode Respawn)
        {
            this.DeveloperConsoleToggle = DeveloperConsoleToggle;
            this.MenuToggle = MenuToggle;
            this.DialogueSubmit = DialogueSubmit;
            this.DialogueCancel = DialogueCancel;
            this.Respawn = Respawn;
        }
    }
}