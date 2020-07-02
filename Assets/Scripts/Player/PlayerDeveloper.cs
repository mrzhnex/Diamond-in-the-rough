using Assets.Scripts.Main;
using Assets.Scripts.Manage;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerDeveloper : MonoBehaviour
    {
        private Menu Menu; 
        public void Start()
        {
            Menu = FindObjectOfType<Menu>();
        }
        public void Update()
        {
            if (Global.GameStage == GameStage.InGame || Global.GameStage == GameStage.PauseMenu)
            {
                if (Input.GetKeyDown(Global.Settings.Controller.OtherBind.MenuToggle))
                {
                    Menu.ToggleGame();
                }
            }
            if (Global.GameStage == GameStage.InGame || Global.GameStage == GameStage.Developer)
            {
                if (Input.GetKeyDown(Global.Settings.Controller.OtherBind.DeveloperConsoleToggle))
                {
                    Global.DeveloperConsoleToggle();
                }
            }

        }
    }
}