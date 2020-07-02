using Assets.Scripts.Manage;
using UnityEngine;

namespace Assets.Scripts.Objects.Room
{
    public class TeleportDoorComponent : MonoBehaviour
    {
        public int TeleportRoomId;

        public void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.collider.name == Global.PlayerOnScene.name && !MapController.CurrentLayout.CurrentRoom.IsLockedDoors)
            {
                MapController.LoadRoomById(TeleportRoomId);
            }
        }
    }
}