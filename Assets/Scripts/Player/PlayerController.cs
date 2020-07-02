using Assets.Scripts.Manage;
using Assets.Scripts.Objects.Layout;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LifeBehavior))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D Rb;
    private readonly float MoveSpeed = 500.0f;
    public Vector2 Movement;
    public Vector3 MousePosition;
    private bool Stoped = false;
    private Transform SpriteTransform;

    public bool CanMovement = true;
    private LifeBehavior LifeBehavior;
    private readonly float TimeToEndSwift = 0.15f;
    private float TimerTwo = 0.0f;
    private LifeForm LifeForm;

    public void Start()
    {
        LifeForm = gameObject.GetComponent<LifeForm>();
        LifeBehavior = GetComponent<LifeBehavior>();
        SpriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        Rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        Movement = Vector2.zero;
        if (Global.GameStage == GameStage.InGame && !gameObject.GetComponent<InVulnerability>())
        {
            if (Input.GetKey(Global.Settings.Controller.MovementBind.Left))
            {
                Movement += Vector2.left;
            }
            if (Input.GetKey(Global.Settings.Controller.MovementBind.Right))
            {
                Movement += Vector2.right;
            }
            if (Input.GetKey(Global.Settings.Controller.MovementBind.Up))
            {
                Movement += Vector2.up;
            }
            if (Input.GetKey(Global.Settings.Controller.MovementBind.Down))
            {
                Movement += Vector2.down;
            }
            if (Movement.x != 0 && Movement.y != 0)
            {
                Movement.x *= 0.8f;
                Movement.y *= 0.8f;
            }

            if (!CanMovement)
            {
                TimerTwo += Time.deltaTime;
                if (TimerTwo > TimeToEndSwift)
                {
                    TimerTwo = 0.0f;
                    CanMovement = true;
                }
            }

            if (LifeBehavior.CanAttackWithDamage() && Input.GetKeyDown(Global.Settings.Controller.MovementBind.Swift) && CanMovement && (Movement.x != 0 || Movement.y != 0))
            {
                LifeBehavior.RemoveHealthForCast();
                Magic.Swift(Movement, gameObject, new object[] { 0.0015f });
                CanMovement = false;
            }
        }
    }

    public void FixedUpdate()
    {
        if (Global.NonFreezingGameStages.Contains(Global.GameStage) && !LifeForm.IsDead)
        {
            Stoped = false;
            if (CanMovement)
                DoMovement();
            Rotate();
        }
        else
        {
            if (!Stoped)
                StopMovementAndRotation();
        }
    }

    private void StopMovementAndRotation()
    {
        Rb.velocity = new Vector2(0.0f, 0.0f);
        transform.rotation = new Quaternion(0.0f, 0.0f, transform.rotation.z, transform.rotation.w);
        Stoped = true;
    }

    private void DoMovement()
    {
        Rb.velocity = new Vector2(Movement.x * MoveSpeed * Time.fixedDeltaTime, Movement.y * MoveSpeed * Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        SpriteTransform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2((MousePosition - SpriteTransform.position).normalized.y, (MousePosition - SpriteTransform.position).normalized.x) * Mathf.Rad2Deg - 92);
    }
}