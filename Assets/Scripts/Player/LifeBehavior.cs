using UnityEngine;

[RequireComponent(typeof(PlayerMagic))]
[RequireComponent(typeof(LifeForm))]
public class LifeBehavior : MonoBehaviour
{
    private LifeForm LifeForm;
    private float Timer = 0.0f;
    private readonly float TimeToHeal = 1.0f;
    private readonly float HealPerSecond = 2.0f;
    private readonly float DamagePerCast = 1.0f;

    public void Start()
    {
        LifeForm = gameObject.GetComponent<LifeForm>();
    }

    public void Update()
    {
        if (!LifeForm.IsDead)
        {
            Timer += Time.deltaTime;
            if (Timer > TimeToHeal)
            {
                Timer = 0.0f;
                LifeForm.Heal(HealPerSecond);
            }
        }
    }

    public void RemoveHealthForCast()
    {
        LifeForm.Damage(DamagePerCast);
    }

    public bool CanAttackWithDamage()
    {
        return DamagePerCast < LifeForm.GetHealth();
    }
}