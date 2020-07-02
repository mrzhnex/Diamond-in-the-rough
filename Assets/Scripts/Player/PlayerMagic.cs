using Assets.Scripts.Manage;
using Assets.Scripts.Objects.Layout;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(LifeBehavior))]
public class PlayerMagic : MonoBehaviour
{
    private readonly float AttackDelay = 0.1f;
    private float Timer = 0.0f;
    private bool CanAttack = true;
    private LifeBehavior LifeBehavior;
    private PlayerController PlayerController;

    public void Start()
    {
        PlayerController = GetComponent<PlayerController>();
        LifeBehavior = GetComponent<LifeBehavior>();
    }
    public void Update()
    {
        if (Global.GameStage == GameStage.InGame && !gameObject.GetComponent<InVulnerability>())
        {
            Timer += Time.deltaTime;
            if (Timer > AttackDelay)
            {
                Timer = 0.0f;
                CanAttack = true;
            }

            if (LifeBehavior.CanAttackWithDamage() && Input.GetMouseButton(0) && CanAttack)
            {
                CanAttack = false;
                LifeBehavior.RemoveHealthForCast();
                Magic.CreateBlueStarsProjectile(new Vector2(transform.position.x, transform.position.y), gameObject, new object[] { new Vector2(PlayerController.MousePosition.x, PlayerController.MousePosition.y), 500.0f });
            }
        }
    }
}