using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Objects.Settings;
using Assets.Scripts.Manage.Developer;
using Assets.Scripts.Manage.Localization;

namespace Assets.Scripts.Manage
{
    public static class Global
    {
        public static Settings Settings = new Settings(new Size(1280, 720), new Difficult("Low Level"), new KeyBind(new MagicBind(KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V), new MovementBind(KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.S, KeyCode.Space), new OtherBind(KeyCode.F1, KeyCode.Escape, KeyCode.Return, KeyCode.Backspace, KeyCode.R)), 1.0f, 1.0f, Language.Russian);

        #region maindata
        private static readonly string SettingsFileName = "Settings.xml";
        public static readonly string CharacterFileName = ".char";
        private static readonly string CharactersFolderName = "Characters";
        public const string HideoutSceneName = "Hideout";
        public const string MenuSceneName = "Menu";
        public static readonly string WallsTag = "Wall/Other";
        
        
        public static readonly string EntityTag = "Entity";
        public static readonly string PlayerTag = "Player";
        public static readonly string EffectTag = "Effect";
        public static readonly string EnemyTag = "Enemy";
        public static readonly string CorpseTag = "Corpse";

        public static readonly string UntaggedTag = "Untagged";
        public static readonly string BackgroundTag = "Background";
        public static readonly string PopupTag = "Popup";
        public static readonly string GUITag = "GUI";
        public static readonly string ButtonResolutionTag = "ButtonResolution";
        public static readonly string ButtonLevelTag = "ButtonLevel";
        public static readonly string ButtonKeyBindingTag = "ButtonKeyBinding";
        public static readonly string MainCameraTag = "MainCamera";

        public static readonly Dictionary<string, float> LayersPositions = new Dictionary<string, float>()
        {
            {BackgroundTag, 1.0f },
            {CorpseTag, 0.5f },
            {WallsTag, 0.0f },
            {EntityTag, 0.0f },
            {PlayerTag, 0.0f },
            {EnemyTag, 0.0f },
            {EffectTag, -1.0f },
            {PopupTag, -2.0f },
            {GUITag, -8.0f },
            {MainCameraTag, -10.0f }
        };
        #endregion

        #region character
        public static string GetCharactersFolder()
        {
            return CharactersFolderName;
        }
        public static string GetSettingsFile()
        {
            return SettingsFileName;
        }
        public static List<Character> Characters = new List<Character>();
        public static Character Character = new Character("Diamond Uncut", 1, 0);
        #endregion

        #region sceneload
        public static GameObject Menu;
        public static GameObject SpawnMenu()
        {
            return Object.Instantiate(Menu, new Vector3(0.0f, 0.0f, LayersPositions[Menu.tag]), Quaternion.identity);
        }

        public static GameObject PlayerUI;
        public static GameObject SpawnPlayerUI()
        {
            return Object.Instantiate(PlayerUI, new Vector3(0.0f, 0.0f, LayersPositions[PlayerUI.tag]), Quaternion.identity);
        }

        public static GameObject MainCamera;
        public static GameObject SpawnMainCamera()
        {
            return Object.Instantiate(MainCamera, new Vector3(0.0f, 0.0f, LayersPositions[MainCamera.tag]), Quaternion.identity);
        }

        public static GameObject Player;
        public static GameObject SpawnPlayerGameObject()
        {
            return Object.Instantiate(Player, new Vector3(0.0f, 0.0f, LayersPositions[Player.tag]), Quaternion.identity);
        }
        public static GameObject PlayerOnScene;

        public static GameObject Hideout;
        public static GameObject SpawnHideout()
        {
            return Object.Instantiate(Hideout, new Vector3(-100.0f, -100.0f, LayersPositions[Hideout.tag]), Quaternion.identity);
        }
        public static GameObject HideoutOnScene;
        #endregion

        #region gameplay
        public static LocalizationComponent LocalizationComponent;
        public static GameStage PreviousGameStage = GameStage.InGame;
        public static GameStage GameStage { get; private set; }
        public static void SetGameStage(GameStage GameStage)
        {
            PreviousGameStage = Global.GameStage;
            Global.GameStage = GameStage;
            Debug("PreviousGameStage is " + PreviousGameStage + " GameStage is " + GameStage);
        }
        public static readonly GameStage[] NonFreezingGameStages = new GameStage[]
        {
            GameStage.InGame, GameStage.CutScene
        };
        public static string ReloadLanguage(Language language)
        {
            if (LocalizationComponent == null)
            {
                return LocalizationManager.Translate("Перевод языка в данный момент невозможен", Settings.Language);
            }
            else
            {
                Settings.Language = language;
                LocalizationComponent.ReloadLanguage();
                Debug("Set language: " + Global.Settings.Language);
                return LocalizationManager.Translate("Язык успешно переведен", Settings.Language);
            }
        }
        #endregion

        #region map
        public static bool IsCreatedPortal = false;

        #endregion

        #region developer
        public static Console Console;
        public static GameObject DeveloperConsole;
        private static GameObject DeveloperConsoleInScene;
        public static void SpawnDeveloperConsole()
        {
            DeveloperConsoleInScene = Object.Instantiate(DeveloperConsole, new Vector3(0.0f, 0.0f, LayersPositions[DeveloperConsole.tag]), Quaternion.identity);
        }
        public static void DeveloperConsoleToggle()
        {
            DeveloperConsoleInScene.SetActive(!DeveloperConsoleInScene.activeSelf);
            if (DeveloperConsoleInScene.activeSelf)
            {
                SetGameStage(GameStage.Developer); 
                Console.RefreshConsole();
            }
            else
            {
                SetGameStage(PreviousGameStage);
            }
        }
        public static void Debug(string message)
        {
            if (Console != null)
                Console.Print(message);
            UnityEngine.Debug.Log(message);
        }
        #endregion

        #region dialogue
        public static GameObject DialogueWindow;

        public static void SpawnDialogueWindow()
        {
            Object.Instantiate(DialogueWindow, new Vector3(0.0f, 0.0f, LayersPositions[DialogueWindow.tag]), Quaternion.identity);
        }

        #endregion

        #region helper
        public static List<T> ToList<T>(this T[,] array)
        {
            List<T> list = new List<T>();

            foreach (T item in array)
            {
                list.Add(item);
            }

            return list;
        }
        public static bool Visible(Vector3 original, Vector3 other, string name, float distance)
        {
            RaycastHit2D[] raycasts = Physics2D.RaycastAll(original, GetDirection(original, other), distance).Where(x => x.collider != null).ToArray();

            if (raycasts.Where(x => x.collider.gameObject.name == name).FirstOrDefault() != default)
            {
                if (raycasts.Where(x => x.collider.gameObject.tag == WallsTag).FirstOrDefault() != default)
                {
                    if (Vector3.Distance(original, raycasts.Where(x => x.collider.gameObject.tag == WallsTag).FirstOrDefault().point) > Vector3.Distance(original, raycasts.Where(x => x.collider.gameObject.name == name).FirstOrDefault().point))
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            }
            return false;
        }
        public static Vector3 GetDirection(Vector3 original, Vector3 other)
        {
            return new Vector3((other - original).normalized.x, (other - original).normalized.y, 0.0f);
        }
        public static Vector2 GetDirection(Vector2 original, Vector2 other)
        {
            return new Vector2((other - original).normalized.x, (other - original).normalized.y);
        }

        #endregion
    }
    public enum GameStage
    {
        InGame, Developer, PauseMenu, Menu, CutScene
    }
}