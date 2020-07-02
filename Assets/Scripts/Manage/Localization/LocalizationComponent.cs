using UnityEngine;
using TMPro;
using Assets.Scripts.Manage.Developer;

namespace Assets.Scripts.Manage.Localization
{
    public class LocalizationComponent : MonoBehaviour
    {
        public void Start()
        {
            Global.LocalizationComponent = this;
        }

        public static void ReloadLanguage()
        {
            foreach (TextMeshProUGUI textMeshProUGUI in FindObjectsOfType<TextMeshProUGUI>())
            {
                if (textMeshProUGUI.GetComponentInParent<Console>())
                    continue;
                textMeshProUGUI.text = LocalizationManager.Translate(textMeshProUGUI.text, Global.Settings.Language);
            }
            foreach (TextMeshPro textMeshPro in FindObjectsOfType<TextMeshPro>())
            {
                textMeshPro.text = LocalizationManager.Translate(textMeshPro.text, Global.Settings.Language);
            }
        }
        public void Update()
        {
            ReloadLanguage();
        }
    }
}