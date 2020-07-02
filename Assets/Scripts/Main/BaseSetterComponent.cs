using Assets.Scripts.Manage;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Objects.Room;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Assets.Scripts.Main
{
    public class BaseSetterComponent : MonoBehaviour
    {
        public void Awake()
        {
            Global.Menu = Resources.Load(nameof(Global.Menu)) as GameObject;
            Global.DeveloperConsole = Resources.Load(nameof(Global.DeveloperConsole)) as GameObject;
            Global.PlayerUI = Resources.Load(nameof(Global.PlayerUI)) as GameObject;
            Global.DialogueWindow = Resources.Load(nameof(Global.DialogueWindow)) as GameObject;
            Global.Hideout = Resources.Load(nameof(Global.Hideout)) as GameObject;

            Global.Player = Resources.Load(nameof(Global.Player)) as GameObject;

            Magic.Explosion = Resources.Load(nameof(Magic.Explosion)) as GameObject;
            Magic.BlueStars = Resources.Load(nameof(Magic.BlueStars)) as GameObject;

            Creatures.ForkLift = Resources.Load(nameof(Creatures.ForkLift)) as GameObject;
            Creatures.Laborant = Resources.Load(nameof(Creatures.Laborant)) as GameObject;
            Creatures.Whell = Resources.Load(nameof(Creatures.Whell)) as GameObject;
            Creatures.Corpse = Resources.Load(nameof(Creatures.Corpse)) as GameObject;
            
            PopupManager.Popup = Resources.Load(nameof(PopupManager.Popup)) as GameObject;

            Subjects.Portal = Resources.Load(nameof(Subjects.Portal)) as GameObject;

            if (SceneManager.GetActiveScene().name == Global.HideoutSceneName)
            {
                Global.SetGameStage(GameStage.InGame);
                MapController.LocationStage = LocationStage.Hideout;
                Global.PlayerOnScene = Global.SpawnPlayerGameObject();
                Global.HideoutOnScene = Global.SpawnHideout();
                Global.PlayerOnScene.transform.position = new Vector3(Global.HideoutOnScene.GetComponentsInChildren<Transform>().Where(x => x.gameObject.name == "Start").First().position.x, Global.HideoutOnScene.GetComponentsInChildren<Transform>().Where(x => x.gameObject.name == "Start").First().position.x, Global.LayersPositions[Global.PlayerOnScene.tag]);
            }
            else
            {
                Global.SetGameStage(GameStage.Menu);
                MapController.LocationStage = LocationStage.Other;
            }
            Global.SpawnMenu();
            Global.SpawnDeveloperConsole();
            Global.SpawnDialogueWindow();
        }
        public void Start()
        {
            List<GameObject> defaultEnemies = new List<GameObject>()
            {
                Creatures.Whell, Creatures.Laborant
            };

            List<GameObject> bossesEnemies = new List<GameObject>()
            {
                Creatures.Laborant
            };

            MapController.RoomPossibleEntities = new RoomPossibleEntities(defaultEnemies, bossesEnemies);
        }
    }
}