using Assets.Scripts.Manage;
using UnityEngine;
using Assets.Scripts.Manage.Localization;
using System.Linq;

namespace Assets.Scripts.Objects.Hideout
{
    public class PortalBehaviour : MonoBehaviour
    {     
        private bool AnimateForward = true;
        private bool Animate = false;
        private readonly float TimeAnimateOneSprite = 0.15f;
        private float Timer = 0.0f;

        public Sprite[] Sprites;
        private SpriteRenderer SpriteRenderer;
        public void Start()
        {
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            Physics2D.IgnoreLayerCollision(2, 11);
        }

        public void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.tag == Global.PlayerTag)
            {
                AnimateForward = true;
                Animate = true;
                ShowDialogue();
            }
        }

        public void Update()
        {
            if (Animate)
            {
                Timer += Time.deltaTime;
                if (Timer > TimeAnimateOneSprite)
                {
                    Timer = 0.0f;

                    if (GetNextSprite() == null)
                    {
                        Animate = false;
                    }
                    else
                    {
                        SpriteRenderer.sprite = GetNextSprite();
                    }

                }
            }
        }

        private Sprite GetNextSprite()
        {
            if (AnimateForward)
            {
                if (Sprites.ToList().IndexOf(SpriteRenderer.sprite) == (Sprites.Length - 1))
                {
                    return null;
                }
                else
                {
                    return Sprites[Sprites.ToList().IndexOf(SpriteRenderer.sprite) + 1];
                }
            }
            else
            {
                if (Sprites.ToList().IndexOf(SpriteRenderer.sprite) == 0)
                {
                    return null;
                }
                else
                {
                    return Sprites[Sprites.ToList().IndexOf(SpriteRenderer.sprite) - 1];
                }
            }
        }


        public void OnTriggerExit2D(Collider2D collider2D)
        {
            if (collider2D.tag == Global.PlayerTag)
            {
                Animate = true;
                AnimateForward = false;
                Dialogue.Controller.HideDialogueWindow();
            }
        }

        private void ShowDialogue()
        {
            Dialogue.Controller.SetCurrentDialogue(new Dialogue.Dialogue()
            {
                AuthorMessage = LocalizationManager.Translate("Активировать портал", Global.Settings.Language) + "?",
                AuthorSprite = gameObject.GetComponentInChildren<SpriteRenderer>().sprite,
                SubmitAction = SubmitAction,
                CancelAction = CancelAction
            });
            Dialogue.Controller.ShowDialogueWindow();
        }

        private void SubmitAction()
        {
            switch (MapController.LocationStage)
            {
                case LocationStage.Hideout:
                    Dialogue.Controller.HideDialogueWindow();
                    Global.SetGameStage(GameStage.CutScene);
                    MapController.GenerateAndStartLayout();
                    Animate = true;
                    AnimateForward = false;
                    break;
                case LocationStage.Map:
                    Dialogue.Controller.HideDialogueWindow();
                    Global.SetGameStage(GameStage.InGame);
                    MapController.LoadHideout();
                    Animate = true;
                    AnimateForward = false;
                    break;
                default:
                    break;
            }
        }
        private void CancelAction()
        {
            Dialogue.Controller.HideDialogueWindow();
            Animate = true;
            AnimateForward = false;
        }
    }
}