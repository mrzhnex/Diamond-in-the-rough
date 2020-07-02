using UnityEngine;
using Assets.Scripts.Manage;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Main
{
    public class StartupComponent : MonoBehaviour
    {
        public void Awake()
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case Global.MenuSceneName:
                    Global.SetGameStage(GameStage.Menu);
                    break;
                case Global.HideoutSceneName:
                    Global.SetGameStage(GameStage.InGame);
                    break;
                default:
                    Global.SetGameStage(GameStage.Developer);                
                    break;
            }
            Global.MainCamera = Resources.Load(nameof(Global.MainCamera)) as GameObject;
            Global.SpawnMainCamera();
        }
        public static void LoadHideout()
        {
            Global.SetGameStage(GameStage.InGame);
            MapController.LocationStage = LocationStage.Hideout;
            if (SceneManager.GetActiveScene().name != Global.HideoutSceneName)
            {
                SceneManager.LoadScene(Global.HideoutSceneName);
                Global.Debug("Load scene: " + Global.HideoutSceneName);
            }
            Global.Debug("Load hideout");
        }
    }
}