using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;
using System.Text.RegularExpressions;
using Assets.Scripts.Manage;
using Assets.Scripts.Objects.Settings;

namespace Assets.Scripts.Main
{
    public class Menu : MonoBehaviour
    {
        #region Buttons
        private GameObject[] ButtonsLevel;
        private GameObject[] ButtonsResolution;
        private GameObject[] ButtonsKeysBinding;
        #endregion

        #region Sliders
        private Slider VolumeEffectsSlider;
        private Slider VolumeMusicSlider;
        #endregion

        #region general
        private GameObject Canvas;
        private GameObject OptionMenu;
        private GameObject NewCharacter;
        private GameObject Loaded;
        private GameObject Main;
        #endregion

        private GameObject PlayerUI;
        private bool KeyBinding = false;
        private string KeyBindingKeyName;
        private bool SliderValueCanChange = false;
        private Button LoadButton;

        public void Awake()
        {
            Global.Settings = SaveLoad.LoadSettings();
            Global.Characters = SaveLoad.LoadCharacters();

            Canvas = gameObject.GetComponentInChildren<Canvas>().gameObject;
            OptionMenu = FindObjectsOfType<Transform>().Where(x => x.gameObject.name == "Option").First().gameObject;
            NewCharacter = FindObjectsOfType<Transform>().Where(x => x.gameObject.name == "NewCharacter").First().gameObject;
            Loaded = FindObjectsOfType<Transform>().Where(x => x.gameObject.name == "Loaded").First().gameObject;
            Main = FindObjectsOfType<Transform>().Where(x => x.gameObject.name == "Main").First().gameObject;


            ButtonsLevel = GameObject.FindGameObjectsWithTag(Global.ButtonLevelTag);
            ButtonsResolution = GameObject.FindGameObjectsWithTag(Global.ButtonResolutionTag);
            ButtonsKeysBinding = GameObject.FindGameObjectsWithTag(Global.ButtonKeyBindingTag);

            VolumeEffectsSlider = FindObjectsOfType<Slider>().Where(x => x.gameObject.name == "Effect").First();
            VolumeMusicSlider = FindObjectsOfType<Slider>().Where(x => x.gameObject.name == "Music").First();

            LoadButton = FindObjectsOfType<Button>().Where(x => x.gameObject.name == "Load Button").First();

            SetUpResolutin();
            SetUpKeys();
            SetUpSliders();
            ReloadValues();

            NewCharacter.SetActive(false);
            OptionMenu.SetActive(false);
            Loaded.SetActive(false);
            if (Global.GameStage == GameStage.InGame)
            {
                PlayerUI = GameObject.FindGameObjectsWithTag(Global.GUITag).Where(x => x.gameObject.name == "PlayerUI(Clone)").First();
                LoadButton.gameObject.SetActive(false);
                FindObjectsOfType<Button>().Where(x => x.gameObject.name == "Create New Character Button").First().gameObject.SetActive(false);
                FindObjectsOfType<Button>().Where(x => x.gameObject.name == "Quit Button").First().gameObject.SetActive(false);
                FindObjectsOfType<Button>().Where(x => x.gameObject.name == "Options Button").First().gameObject.SetActive(false);
                Canvas.SetActive(false);
            }
            else
            {
                if (Global.Characters.Count == 0)
                {
                    LoadButton.gameObject.SetActive(false);
                }
                FindObjectsOfType<Button>().Where(x => x.gameObject.name == "Continue Button").First().gameObject.SetActive(false);
                FindObjectsOfType<Button>().Where(x => x.gameObject.name == "Main Menu Button").First().gameObject.SetActive(false);
            }
        }

        public void Update()
        {
            if (KeyBinding)
            {
                if (Input.anyKeyDown)
                {
                    switch (KeyBindingKeyName)
                    {
                        case "Left":
                            Global.Settings.Controller.MovementBind.Left = (KeyCode)Enum.Parse(typeof(KeyCode), GetKeyStringByInputString(Input.inputString));
                            break;
                        case "Right":
                            Global.Settings.Controller.MovementBind.Right = (KeyCode)Enum.Parse(typeof(KeyCode), GetKeyStringByInputString(Input.inputString));
                            break;
                        case "Down":
                            Global.Settings.Controller.MovementBind.Down = (KeyCode)Enum.Parse(typeof(KeyCode), GetKeyStringByInputString(Input.inputString));
                            break;
                        case "Up":
                            Global.Settings.Controller.MovementBind.Up = (KeyCode)Enum.Parse(typeof(KeyCode), GetKeyStringByInputString(Input.inputString));
                            break;
                        case "Power":
                            Global.Settings.Controller.MagicBind.Power = (KeyCode)Enum.Parse(typeof(KeyCode), GetKeyStringByInputString(Input.inputString));
                            break;
                        case "Speed":
                            Global.Settings.Controller.MagicBind.Speed = (KeyCode)Enum.Parse(typeof(KeyCode), GetKeyStringByInputString(Input.inputString));
                            break;
                        case "Area":
                            Global.Settings.Controller.MagicBind.Area = (KeyCode)Enum.Parse(typeof(KeyCode), GetKeyStringByInputString(Input.inputString));
                            break;
                        case "Stability":
                            Global.Settings.Controller.MagicBind.Stability = (KeyCode)Enum.Parse(typeof(KeyCode), GetKeyStringByInputString(Input.inputString));
                            break;
                    }

                    Debug.Log("KeyCode: " + Input.inputString);
                    KeyBinding = false;
                    SetUpKeys();
                }
            }
        }

        #region loaded
        public void StartGame()
        {
            if (Global.Characters.Where(x => x.Name == GetChacacterNameByDropdownValue(Loaded.GetComponentInChildren<TMP_Dropdown>().options[Loaded.GetComponentInChildren<TMP_Dropdown>().value].text)).FirstOrDefault() == default)
            {
                Global.Debug("Character with name " + GetChacacterNameByDropdownValue(Loaded.GetComponentInChildren<TMP_Dropdown>().options[Loaded.GetComponentInChildren<TMP_Dropdown>().value].text) + " not found");
                return;
            }
            Global.Character = Global.Characters.Where(x => x.Name == GetChacacterNameByDropdownValue(Loaded.GetComponentInChildren<TMP_Dropdown>().options[Loaded.GetComponentInChildren<TMP_Dropdown>().value].text)).FirstOrDefault();

            if (Global.Character.Level == 1)
            {
                StartNewGame();
            }
            else
            {
                StartupComponent.LoadHideout();
            }
        }

        private void StartNewGame()
        {
            Global.Debug("Load first cut scene...");
            //первоначальная кат сцена
            StartupComponent.LoadHideout();
        }
        #endregion

        #region pause

        public void ToggleGame()
        {    
            switch (Global.GameStage)
            {
                case GameStage.InGame:
                    PlayerUI.SetActive(false);
                    Canvas.SetActive(true);
                    Dialogue.Controller.HideDialogueWindow();
                    Global.SetGameStage(GameStage.PauseMenu);
                    break;
                case GameStage.Menu:
                    break;
                case GameStage.PauseMenu:
                    Canvas.SetActive(false);
                    PlayerUI.SetActive(true);
                    Global.SetGameStage(GameStage.InGame);
                    break;
                default:
                    break;
            }
        }

        public void MainMenu()
        {
            Global.SetGameStage(GameStage.Menu);
            SceneManager.LoadScene(Global.MenuSceneName);
            Global.Debug("Load scene: " + Global.MenuSceneName);
        }
        #endregion

        #region main
        public void CharactersLoad()
        {
            Loaded.GetComponentsInChildren<TMP_Dropdown>().First().options.Clear();
            foreach (Character character in SaveLoad.LoadCharacters())
            {
                Loaded.GetComponentsInChildren<TMP_Dropdown>().First().options.Add(new TMP_Dropdown.OptionData(character.Name + " (" + Manage.Localization.LocalizationManager.Translate("уровень", Global.Settings.Language) + ": " + character.Level + ")"));
            }
        }

        public void CreateNewCharacter()
        {
            Character character = new Character(FindObjectsOfType<Text>().Where(x => x.gameObject.name == "CharacterNameText").First().text, 1, 0);
            Global.Characters = SaveLoad.LoadCharacters();
            if (Global.Characters.Where(x => x.Name == character.Name).FirstOrDefault() != default)
            {
                Global.Debug("Character with name '" + character.Name + "' is already exist");
                return;
            }
            if (character.Name == null || character.Name == string.Empty || character.Name.Contains(" "))
            {
                return;
            }
            if (character.Name.Length < 6 || character.Name.Length > 12)
            {
                Global.Debug("Character name length must be more than 5 and less than 13");
                return;
            }

            SaveLoad.SaveCharacter(character);
            Global.Characters = SaveLoad.LoadCharacters();
            if (Global.Characters.Count > 0)
            {
                LoadButton.gameObject.SetActive(true);
            }
            Global.Debug("Create new character with name '" + character.Name + "'");
            Main.SetActive(true);
            NewCharacter.SetActive(false);
            
        }
        public void QuitGame()
        {
            Global.Debug("Quit game");
            Application.Quit();
        }
        #endregion

        #region options
        public void SetKeyBindingMode(Button button)
        {

            KeyBindingKeyName = button.gameObject.name;
            KeyBinding = true;
        }
        public void ChangeVolumeEffects(Slider slider)
        {
            if (SliderValueCanChange)
            {
                Global.Settings.VolumeEffect = slider.value;
                Global.Debug("set effects value: " + slider.value);
            }
        }
        public void ChangeVolumeMusic(Slider slider)
        {
            if (SliderValueCanChange)
            {
                Global.Settings.VolumeMusic = slider.value;
                Global.Debug("set music value: " + slider.value);
            }
        }
        public void SetResolution(Button button)
        {
            Global.Settings.Resolution.Width = int.Parse(GetSizesByResolution(button.name)[0]);
            Global.Settings.Resolution.Height = int.Parse(GetSizesByResolution(button.name)[1]);
            SetUpResolutin();
            ReloadValues();
            Global.Debug("Set screen resolution: " + Global.Settings.Resolution.Width + " " + Global.Settings.Resolution.Height);
        }
        public void SetDifficult(Button button)
        {
            switch (button.name)
            {
                case "High Level":
                    Global.Settings.Level = new Difficult(button.name);
                    break;
                case "Medium Level":
                    Global.Settings.Level = new Difficult(button.name);
                    break;
                case "Low Level":
                    Global.Settings.Level = new Difficult(button.name);
                    break;
            }
            Global.Debug("Set difficult level: " + Global.Settings.Level.DifficultName);
            ReloadValues();
        }
        public void SaveAllSettings()
        {
            SaveLoad.SaveSettings();
        }
        #endregion

        #region helper
        private string GetChacacterNameByDropdownValue(string value)
        {
            return value.Remove(value.IndexOf(" ("));
        }
        private string GetKeyStringByInputString(string inpuString)
        {
            if (Regex.IsMatch(inpuString, @"\p{IsCyrillic}"))
            {
                return "A";
            }
            return inpuString.ToCharArray()[0].ToString().ToUpper() + inpuString.Substring(1);
        }
        private string[] GetSizesByResolution(string resolution)
        {
            return resolution.Split('x');
        }
        private string GetResolutionBySize(int width, int height)
        {
            return string.Concat(width, "x", height);
        }
        private void SetUpResolutin()
        {
            Screen.SetResolution(Global.Settings.Resolution.Width, Global.Settings.Resolution.Height, FullScreenMode.Windowed);
        }
        private void ReloadValues()
        {
            foreach (GameObject gameObject in ButtonsLevel)
            {
                gameObject.GetComponentInChildren<TextMeshProUGUI>().faceColor = new Color32(255, 255, 255, 255);
            }

            foreach (GameObject gameObject1 in ButtonsResolution)
            {
                gameObject1.GetComponentInChildren<TextMeshProUGUI>().faceColor = new Color32(255, 255, 255, 255);
            }

            ButtonsLevel.Where(x => x.name == Global.Settings.Level.DifficultName).First().GetComponentInChildren<TextMeshProUGUI>().faceColor = new Color32(196, 164, 61, 100);
            ButtonsResolution.Where(x => x.name == GetResolutionBySize(Global.Settings.Resolution.Width, Global.Settings.Resolution.Height)).First().GetComponentInChildren<TextMeshProUGUI>().faceColor = new Color32(196, 164, 61, 100);
        }
        private void SetUpSliders()
        {
            VolumeEffectsSlider.value = Global.Settings.VolumeEffect;
            VolumeMusicSlider.value = Global.Settings.VolumeMusic;
            SliderValueCanChange = true;
        }
        private void SetUpKeys()
        {
            foreach (GameObject gameObject in ButtonsKeysBinding)
            {
                switch (gameObject.name)
                {
                    case "Left":
                        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Global.Settings.Controller.MovementBind.Left.ToString();
                        break;
                    case "Right":
                        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Global.Settings.Controller.MovementBind.Right.ToString();
                        break;
                    case "Down":
                        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Global.Settings.Controller.MovementBind.Down.ToString();
                        break;
                    case "Up":
                        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Global.Settings.Controller.MovementBind.Up.ToString();
                        break;
                    case "Power":
                        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Global.Settings.Controller.MagicBind.Power.ToString();
                        break;
                    case "Speed":
                        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Global.Settings.Controller.MagicBind.Speed.ToString();
                        break;
                    case "Area":
                        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Global.Settings.Controller.MagicBind.Area.ToString();
                        break;
                    case "Stability":
                        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = Global.Settings.Controller.MagicBind.Stability.ToString();
                        break;
                }
            }
        }
        #endregion
    }
}