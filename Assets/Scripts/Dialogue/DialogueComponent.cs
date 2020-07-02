using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts.Manage;

namespace Assets.Scripts.Dialogue
{
    public class DialogueComponent : MonoBehaviour
    {
        public Dialogue Dialogue { get; set; }
        private Image Image { get; set; }
        private TextMeshProUGUI TextMeshProUGUIAuthorMessage { get; set; }
        public Sprite SkullSprite;

        public void Start()
        {
            Image = GetComponentsInChildren<Image>().Where(x => x.gameObject.name == "AuthorSprite").First();
            TextMeshProUGUIAuthorMessage = GetComponentsInChildren<TextMeshProUGUI>().Where(x => x.gameObject.name == "AuthorMessage").First();
            
            Controller.DialogueComponent = this;
            gameObject.SetActive(false);
        }

        public void RefreshDialogue()
        {
            Image.sprite = Dialogue.AuthorSprite;
            TextMeshProUGUIAuthorMessage.text = Dialogue.AuthorMessage;
        }

        public void Update()
        {
            if (gameObject.activeSelf)
            {
                if (Input.GetKeyDown(Global.Settings.Controller.OtherBind.DialogueSubmit))
                {
                    Controller.SubmitDialogue();
                }
                if (Input.GetKeyDown(Global.Settings.Controller.OtherBind.DialogueCancel))
                {
                    Controller.CancelDialogue();
                }
            }
        }
    }
}