using UnityEngine;
using Assets.Scripts.Manage;

namespace Assets.Scripts.Objects.Layout
{
    public class GenerateLayoutAsyncComponent : MonoBehaviour
    {
        private readonly float TimeToGenerateRoom = 0.03f;
        private float Timer = 0.0f;
        private int KlasterCount = 0;

        public void Start()
        {
            MapController.CurrentLayout = new Layout((int)Global.Character.Level);
        }

        public void Update()
        {
            Timer += Time.deltaTime;

            if (Timer > TimeToGenerateRoom)
            {
                Timer = 0.0f;

                if (MapController.CurrentLayout != null)
                {
                    if (KlasterCount != 1)
                    {
                        MapController.CurrentLayout.SetKlasters(MapController.CurrentLayout.GetKlasters(out KlasterCount));
                    }
                    else
                    {
                        foreach (Room.Room room in MapController.CurrentLayout.Rooms)
                        {
                            if (!room.IsDoorsCreate)
                                room.CreateDoors();
                        }
                        if (CheckRoomsAllDoorsCreated(MapController.CurrentLayout.Rooms))
                        {
                            Destroy(gameObject.GetComponent<GenerateLayoutAsyncComponent>());
                        }
                    }
                }
            }
        }
        private bool CheckRoomsAllDoorsCreated(Room.Room[,] rooms)
        {
            foreach (Room.Room room in rooms)
            {
                if (!room.IsDoorsCreate)
                    return false;
            }
            return true;
        }
    }
}
