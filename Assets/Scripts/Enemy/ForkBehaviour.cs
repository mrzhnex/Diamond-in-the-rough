using UnityEngine;
using Assets.Scripts.Manage;
using Assets.Scripts.Effects;
using System.Linq;
using Assets.Scripts.Objects.Layout;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(LifeForm))]
    public class ForkBehaviour : MonoBehaviour
    {
        private readonly float DistanceToVision = 1.0f;
        private GameObject Owner;

        private bool VisibleTarget;

        private float RotateSpeed = 150.0f;
        private Transform SpriteTransform;

        private readonly float MoveSpeed = 200.0f;
        private Vector2 Movement;
        private Rigidbody2D Rb;
        private System.Random Random = new System.Random();

        private static readonly float TimeToNewDirection = 1.0f;
        private float Timer = TimeToNewDirection;

        private LifeForm LifeForm;

        public void Start()
        {
            LifeForm = GetComponent<LifeForm>();
            SpriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
            Movement = Directions[Random.Next(Directions.Length)];
            Rb = GetComponent<Rigidbody2D>();
        }

        public void Update()
        {
            if (Global.NonFreezingGameStages.Contains(Global.GameStage))
            {
                if (Owner == null)
                {
                    LifeForm.Damage(LifeForm.MaxHealth);
                }
                else
                {
                    SpriteTransform.Rotate(0.0f, 0.0f, RotateSpeed * Time.deltaTime, Space.Self);

                    if (!VisibleTarget && MapController.CurrentLayout.CurrentRoom.GetEntities().Where(x => x.GameObjectOnScene == gameObject).FirstOrDefault() != default)
                    {
                        foreach (Entity entity in MapController.CurrentLayout.CurrentRoom.GetEntities())
                        {
                            if (entity.EntityStage == EntityStage.Dead || entity.GameObjectOnScene.GetComponent<InVulnerability>() || !entity.IsEnemy(MapController.CurrentLayout.CurrentRoom.GetEntities().Where(x => x.GameObjectOnScene == gameObject).FirstOrDefault().EntityType))
                                continue;
                            if (Global.Visible(transform.position, entity.GameObjectOnScene.transform.position, gameObject.name, DistanceToVision))
                            {
                                VisibleTarget = true;
                                RotateSpeed = 3000.0f;
                                SetObjectToProjectile(entity.GameObjectOnScene);
                                break;
                            }
                        }
                    }
                    
                }
            }
        }

        public void FixedUpdate()
        {
            if (Global.NonFreezingGameStages.Contains(Global.GameStage) && !VisibleTarget)
            {
                Timer += Time.fixedDeltaTime;
                if (Timer > TimeToNewDirection)
                {
                    Timer = 0.0f;
                    DoMovement();
                }
            }
        }

        public void SetOwner(GameObject Owner)
        {
            this.Owner = Owner;
        }

        private void DoMovement()
        {
            Movement = Directions[Random.Next(Directions.Length)];
            if (Movement.x != 0 && Movement.y != 0)
            {
                Movement.x *= 0.8f;
                Movement.y *= 0.8f;
            }
            Rb.velocity = new Vector2(Movement.x * MoveSpeed * Time.fixedDeltaTime, Movement.y * MoveSpeed * Time.fixedDeltaTime);
        }

        private readonly Vector2[] Directions = new Vector2[]
        {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right, Vector2.up + Vector2.left, Vector2.up + Vector2.right, Vector2.down + Vector2.left, Vector2.down + Vector2.right
        };

        private void SetObjectToProjectile(GameObject target)
        {
            gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
            gameObject.AddComponent<Projectile>();
            gameObject.GetComponent<Projectile>().SetOwner(Owner);
            gameObject.GetComponent<Projectile>().Direction = Global.GetDirection(transform.position, target.transform.position);
            gameObject.GetComponent<Projectile>().ProjectileSpeed = 300.0f;
        }

        public void OnDestroy()
        {
            if (Owner != null)
            {
                Owner.GetComponent<ForkOwnerComponent>().ForkCount--;
            }
        }
    }
}