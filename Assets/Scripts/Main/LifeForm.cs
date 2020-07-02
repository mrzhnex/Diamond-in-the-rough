using Assets.Scripts.Dialogue;
using Assets.Scripts.Manage;
using Assets.Scripts.Manage.Localization;
using System.Linq;
using TMPro;
using UnityEngine;
using Assets.Scripts.Objects.Room;

public class LifeForm : MonoBehaviour
{
    public float MaxHealth = 100.0f;
    public bool IsHiddenLifeForm = false;
    private Color MaxColor = Color.green;
    private Color MinColor = Color.red;
    private float Health;
    private Color HealthColor;

    private TextMeshProUGUI TextMeshProUGUI;

    [HideInInspector]
    public bool IsDead;
    public Sprite[] DeathSprite;
    private System.Random Random = new System.Random();
    private Quaternion AliveQuaternion;

    private bool IsPlayer;
    private Sprite BodySprite;

    public void Start()
    {
        Health = MaxHealth;
        TextMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        if (gameObject == Global.PlayerOnScene)
        {
            IsPlayer = true;
            TextMeshProUGUI.text = string.Empty;
            BodySprite = GetComponentInChildren<SpriteRenderer>().sprite;
        }
    }

    public float GetHealth()
    {
        return Health;
    }

    public void Update()
    {
        if (!IsHiddenLifeForm)
        {
            if (IsPlayer)
            {
                if (IsDead)
                {
                    if (Input.GetKeyDown(Global.Settings.Controller.OtherBind.Respawn))
                    {
                        IsDead = false;
                        Controller.HideDialogueWindow();
                        GetComponentInChildren<SpriteRenderer>().sprite = BodySprite;
                        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                        gameObject.transform.rotation = AliveQuaternion;
                        MapController.Respawn();
                    }
                }
            }
            else
            {
                HealthColor = Color.Lerp(MaxColor, MinColor, (MaxHealth - Health) / MaxHealth);
                TextMeshProUGUI.text = Health + " / " + MaxHealth;
                TextMeshProUGUI.color = HealthColor;
            }
        }
    }

    public void Damage(float ammount)
    {
        if (IsPlayer)
        {
            if (MapController.LocationStage == LocationStage.Map && Global.GameStage == GameStage.InGame && !IsDead)
            {
                PopupManager.CreatePopup(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), ammount.ToString());
                if ((Health - ammount) <= 0.0f)
                {
                    Health = 0.0f;
                    Die();
                }
                else
                {
                    Health -= ammount;
                }
            }
        }
        else
        {
            if (Global.GameStage == GameStage.InGame && !IsDead)
            {
                if ((Health - ammount) <= 0.0f)
                {
                    Health = 0.0f;
                    Die();
                }
                else
                {
                    Health -= ammount;
                }
            }
        }
    }

    public void Heal(float ammount)
    {
        Health += ammount;
        if (Health > MaxHealth)
            Health = MaxHealth;
    }

    private void Die()
    {
        IsDead = true;
        Global.Debug("LifeForm " + gameObject.name + " was slain");
        if (MapController.LocationStage == LocationStage.Map)
        {
            if (IsPlayer)
            {
                gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                AliveQuaternion = gameObject.transform.rotation;
                gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y, Random.Next(-180, 180));
                GetComponentInChildren<SpriteRenderer>().sprite = DeathSprite[Random.Next(DeathSprite.Length)];
                MapController.CurrentLayout.CurrentRoom.RemoveEntityFromScene(gameObject);
                Global.SetGameStage(GameStage.CutScene);
                Controller.SetCurrentDialogue(new Dialogue()
                {
                    AuthorMessage = LocalizationManager.Translate("Вы умерли", Global.Settings.Language) + "!\n" + LocalizationManager.Translate("Нажмите", Global.Settings.Language) + " " + Global.Settings.Controller.OtherBind.Respawn.ToString() + " " + LocalizationManager.Translate("для возрождения", Global.Settings.Language),
                    AuthorSprite = Controller.DialogueComponent.SkullSprite
                });
                Controller.ShowDialogueWindow();
            }
            else
            {
                if (MapController.CurrentLayout.CurrentRoom.GetEntities().Where(x => x.GameObjectOnScene == gameObject).FirstOrDefault() != default)
                {
                    MapController.CurrentLayout.GetRoomById(MapController.CurrentLayout.CurrentRoom.GetEntities().Where(x => x.GameObjectOnScene == gameObject).FirstOrDefault().RoomId).SetEntityAsCorpses(gameObject, Creatures.SpawnCorpse(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), DeathSprite[Random.Next(DeathSprite.Length)]));
                }
                else
                {
                    foreach (Room room in MapController.CurrentLayout.Rooms)
                    {
                        if (room.GetEntities().Where(x => x.GameObjectOnScene == gameObject).FirstOrDefault() != default)
                        {
                            room.SetEntityAsCorpses(gameObject, Creatures.SpawnCorpse(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), DeathSprite[Random.Next(DeathSprite.Length)]));
                        }
                    }
                }
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

}