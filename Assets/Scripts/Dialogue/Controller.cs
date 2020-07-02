namespace Assets.Scripts.Dialogue
{
    public static class Controller
    {
        public static DialogueComponent DialogueComponent { get; set; }

        public static void SetCurrentDialogue(Dialogue dialogue)
        {
            DialogueComponent.Dialogue = dialogue;
            DialogueComponent.RefreshDialogue();
        }

        public static void SubmitDialogue()
        {
            DialogueComponent.Dialogue.SubmitAction();
        }

        public static void CancelDialogue()
        {
            DialogueComponent.Dialogue.CancelAction();
        }

        public static void ShowDialogueWindow()
        {
            DialogueComponent.gameObject.SetActive(true);
        }

        public static void HideDialogueWindow()
        {
            DialogueComponent.gameObject.SetActive(false);
        }
    }
}