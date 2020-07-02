using Assets.Scripts.Manage;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Objects.Layout;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(ForkOwnerComponent))]
    [RequireComponent(typeof(LifeForm))]
    public class LaborantAction : MonoBehaviour
    {
        private float Timer = 0.0f;
        private ForkOwnerComponent ForkOwnerComponent;
        private float RotateSpeed = 200.0f;
        private Transform SpriteTransform;
        public void Start()
        {
            ForkOwnerComponent = GetComponent<ForkOwnerComponent>();
            SpriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        }

        public void Update()
        {
            if (Global.NonFreezingGameStages.Contains(Global.GameStage))
            {
                Timer += Time.deltaTime;
                DoRotate();
                if (ForkOwnerComponent.ForkCount >= ForkOwnerComponent.MaxForkCount)
                {
                    return;
                }
                if (Timer > ForkOwnerComponent.TimeToAttack)
                {
                    Timer = 0.0f;
                    if (MapController.CurrentLayout.CurrentRoom.GetEntities().Where(x => x.GameObjectOnScene == gameObject).FirstOrDefault() != default)
                    {
                        for (int i = 0; i < MapController.CurrentLayout.CurrentRoom.GetEntities().Count; i++)
                        {
                            if (MapController.CurrentLayout.CurrentRoom.GetEntities()[i].EntityStage == EntityStage.Dead || MapController.CurrentLayout.CurrentRoom.GetEntities()[i].GameObjectOnScene.GetComponent<InVulnerability>())
                                continue;
                            if (MapController.CurrentLayout.CurrentRoom.GetEntities()[i].IsEnemy(MapController.CurrentLayout.CurrentRoom.GetEntities().Where(x => x.GameObjectOnScene == gameObject).FirstOrDefault().EntityType))
                            {
                                MapController.CurrentLayout.CurrentRoom.AddEntityToScene(new Entity(MapController.CurrentLayout.CurrentRoom.GetEntities().Where(x => x.GameObjectOnScene == gameObject).FirstOrDefault().RoomId, Creatures.CreateFork(transform.position, MapController.CurrentLayout.CurrentRoom.GetEntities()[i].GameObjectOnScene.transform.position, gameObject), MapController.CurrentLayout.CurrentRoom.GetEntities().Where(x => x.GameObjectOnScene == gameObject).FirstOrDefault().EntityType, EntityStage.Alive));
                                ForkOwnerComponent.ForkCount++;
                            }
                        }
                    }
                    
                }
            }
        }

        public void DoRotate()
        {
            SpriteTransform.Rotate(0.0f, 0.0f, RotateSpeed * Time.deltaTime, Space.Self);
        }
    }
}