using Assets.Scripts.Manage;
using UnityEngine;
using System.Linq;

namespace Assets.Scripts.Objects.Layout
{
    [RequireComponent(typeof(PlayerUI))]
    public class ProcessLayoutAsyncComponent : MonoBehaviour
    {
        private int EnemyCount { get; set; }
        private bool IsLayoutEnd { get; set; }
        private GameObject Portal;

        public void Update()
        {
            if (!IsLayoutEnd)
            {
                if (!MapController.CurrentLayout.CurrentRoom.IsLockedDoors)
                    return;
                if (MapController.CurrentLayout.CurrentRoom.GetEntities().Where(x => x.EntityStage == EntityStage.Alive && x.EntityType == EntityType.Enemy).ToList().Count == 0)
                {
                    Global.Debug("Unlock doors");
                    MapController.UnlockTeleportDoors(MapController.CurrentLayout.CurrentRoom);
                    EnemyCount = 0;
                    foreach (Room.Room room in MapController.CurrentLayout.Rooms)
                    {
                        if (MapController.CurrentLayout.CurrentRoom == room)
                            continue;
                        EnemyCount += room.GetEntities().Where(x => x.EntityStage == EntityStage.Alive && x.EntityType == EntityType.Enemy).ToList().Count;
                    }
                    if (EnemyCount == 0)
                    {
                        IsLayoutEnd = true;
                        Portal = Subjects.SpawnPortal(Global.PlayerOnScene.transform.position);                        
                    }

                }
            }
        }

        public void OnDestroy()
        {
            if (Portal != null)
                Destroy(Portal);
        }

    }
}