using Assets.Scripts.Manage;
using UnityEngine;

namespace Assets.Scripts.Objects.Room
{
    public class BlockSetterComponent : MonoBehaviour
    {
        public Sprite[] Sprites;
        public Sprite TeleportDoor;
        public void Awake()
        {
            Subjects.Empty = Resources.Load(nameof(Subjects.Empty)) as GameObject;
            Subjects.Sprites = Sprites;
            Subjects.TeleportDoor = TeleportDoor;
        }
    }
}