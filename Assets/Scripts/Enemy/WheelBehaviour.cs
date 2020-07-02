using Assets.Scripts.Manage;
using Assets.Scripts.Objects.Layout;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class WheelBehaviour : MonoBehaviour
    {
        private float RotateSpeed = 150.0f;
        private float MoveSpeed = 2.0f;
        private readonly float DistanceToExplode = 0.3f;

        private Vector3 Direction;

        private Transform SpriteTransform;
        private GameObject Target;
        private float Timer = 0.0f;
        private readonly float TimeToExplode = 0.5f;
        public void Start()
        {
            Direction = Global.GetDirection(transform.position, Global.PlayerOnScene.transform.position);
            SpriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        }

        public void Update()
        {
            Timer += Time.deltaTime;
            if (Global.NonFreezingGameStages.Contains(Global.GameStage))
            {
                SpriteTransform.Rotate(0.0f, 0.0f, RotateSpeed * Time.deltaTime, Space.Self);

                if (MapController.CurrentLayout.CurrentRoom.GetEntities().Where(x => x.GameObjectOnScene == gameObject).FirstOrDefault() != default)
                {
                    foreach (Entity entity in MapController.CurrentLayout.CurrentRoom.GetEntities())
                    {
                        if (entity.EntityStage == EntityStage.Dead || entity.GameObjectOnScene.GetComponent<InVulnerability>())
                            continue;
                        if (entity.IsEnemy(MapController.CurrentLayout.CurrentRoom.GetEntities().Where(x => x.GameObjectOnScene == gameObject).FirstOrDefault().EntityType))
                        {
                            Target = gameObject;
                            Movement();
                            RotateSpeed = 2000.0f;
                            MoveSpeed = 3.0f;
                        }
                        else
                        {
                            RotateSpeed = 150.0f;
                        }
                    }
                }
            }
        }
        private void Movement()
        {
            Direction = Global.GetDirection(transform.position, Global.PlayerOnScene.transform.position);
            transform.position += Direction * MoveSpeed * Time.deltaTime;
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(Target.transform.position.x, Target.transform.position.y)) < DistanceToExplode && Timer > TimeToExplode)
            {
                Timer = 0.0f;
                Magic.CreateExplosion(transform.position, gameObject);
            }
        }
    }
}