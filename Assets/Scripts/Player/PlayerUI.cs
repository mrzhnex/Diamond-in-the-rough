using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Assets.Scripts.Manage;
using TMPro;

[RequireComponent(typeof(LifeForm))]
[RequireComponent(typeof(LifeBehavior))]
public class PlayerUI : MonoBehaviour
{
    private GameObject UI;

    private Slider Slider;
    private Image Image;
    private TextMeshProUGUI CharacterName;

    private readonly string ImageHealth = "Health";
    private readonly string CharacterNameGUIName = "CharacterName";
    private LifeForm LifeForm;
    private string TempInfoText;

    public void Awake()
    {
        UI = Global.SpawnPlayerUI();
    }

    public void Start()
    {
        LifeForm = gameObject.GetComponent<LifeForm>();

        Slider = UI.GetComponentInChildren<Slider>();
        Slider.maxValue = LifeForm.MaxHealth;
        Slider.minValue = 0.0f;

        Image = UI.GetComponentsInChildren<Image>().Where(x => x.name == ImageHealth).First();
        CharacterName = UI.GetComponentsInChildren<TextMeshProUGUI>().Where(x => x.name == CharacterNameGUIName).First();
        CharacterName.text = Global.Character.Name;        
    }

    public void Update()
    {
        if (Global.NonFreezingGameStages.Contains(Global.GameStage))
        {
            Slider.value = LifeForm.GetHealth();
            Image.fillAmount = LifeForm.GetHealth() / LifeForm.MaxHealth;
        }
    }
}