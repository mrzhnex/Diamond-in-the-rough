using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts.Manage.Developer
{
    public class Console : MonoBehaviour
    {
        private string ConsoleLog = "Welcome to debug console...\n";
        private float ConsoleWidth = 5.0f;
        private readonly float StringWidthStep = 5.0f;
        private float DefaultConsoleWidth;
        public TextMeshProUGUI ConsoleLogScreenTMP;
        public TMP_InputField ConsoleInputField;
        public ScrollRect ScrollRect;

        public void Awake()
        {
            Global.Console = this;
            ConsoleLogScreenTMP.text = ConsoleLog;
            gameObject.SetActive(false);
            DefaultConsoleWidth = ConsoleLogScreenTMP.rectTransform.sizeDelta.y;
        }

        #region Helpers
        public void RefreshConsole()
        {
            ConsoleLogScreenTMP.text = ConsoleLog;
            ConsoleLogScreenTMP.rectTransform.sizeDelta = new Vector2(ConsoleLogScreenTMP.rectTransform.sizeDelta.x, DefaultConsoleWidth + ConsoleWidth);
            ScrollRect.normalizedPosition = new Vector2(ScrollRect.normalizedPosition.x, 0);
        }
        public void Print(string line)
        {
            ConsoleLog += line + "\n";
            ConsoleWidth += StringWidthStep;
            if (Global.GameStage == GameStage.Developer)
            {
                RefreshConsole();
            }
        }
        #endregion

    }
}
