using Assets.Scripts.Manage;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    [RequireComponent(typeof(LifeForm))]
    public class Projectile : MonoBehaviour
    {
        private GameObject Owner;
        public Vector3 Direction;
        public float ProjectileSpeed = 500f;

        private Rigidbody2D Rb;
        private LifeForm LifeForm;

        public void Start()
        {
            if (!GetComponent<Rigidbody2D>())
            {
                gameObject.AddComponent<Rigidbody2D>();
            }
            Rb = GetComponent<Rigidbody2D>();
            LifeForm = GetComponent<LifeForm>();
        }

        public void FixedUpdate()
        {
            if (Global.NonFreezingGameStages.Contains(Global.GameStage))
            {
                DoMovement();
            }
        }

        private void DoMovement()
        {
            Rb.velocity = new Vector2(Direction.x * ProjectileSpeed * Time.fixedDeltaTime, Direction.y * ProjectileSpeed * Time.fixedDeltaTime);
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (Owner != null)
            {
                if (collision.gameObject.tag == Owner.tag)
                {
                    return;
                }
                if (collision.gameObject.GetComponent<Projectile>() != null)
                {
                    if (collision.gameObject.GetComponent<Projectile>().Owner.name == Owner.name)
                    {
                        return;
                    }
                }
            }
            Magic.CreateExplosion(transform.position, Owner, new object[] { gameObject });
            LifeForm.Damage(LifeForm.MaxHealth);
        }
        public void SetOwner(GameObject Owner)
        {
            this.Owner = Owner;
        }
    }
}