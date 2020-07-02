using Assets.Scripts.Manage;
using UnityEngine;

namespace Assets.Scripts.Objects.Layout
{
    public class LoadLayoutAsyncComponent : MonoBehaviour
    {
        private readonly float TimeLoadRoom = 0.03f;
        private float Timer = 0.0f;

        public void Update()
        {
            if (MapController.CurrentLayout == null || gameObject.GetComponent<GenerateLayoutAsyncComponent>())
            {
                return;
            }
            Timer += Time.deltaTime;
            if (Timer > TimeLoadRoom)
            {
                Timer = 0.0f;
                foreach (Room.Room room in MapController.CurrentLayout.Rooms)
                {
                    if (!room.IsLoaded)
                    {
                        room.Load();
                        break;
                    }
                }
                if (CheckRoomsAllLoaded(MapController.CurrentLayout.Rooms))
                {
                    MapController.Respawn();
                    gameObject.AddComponent<ProcessLayoutAsyncComponent>();
                    Destroy(gameObject.GetComponent<LoadLayoutAsyncComponent>());
                }
            }

        }

        private bool CheckRoomsAllLoaded(Room.Room[,] rooms)
        {
            foreach (Room.Room room in rooms)
            {
                if (!room.IsLoaded)
                    return false;
            }
            return true;
        }
    }
}