using Assets.Scripts.Objects.Room;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Manage
{
    public static class Subjects
    {
        public static Sprite[] Sprites;
        public static GameObject Empty;
        public static Sprite TeleportDoor;
        public static GameObject Portal;

        public static GameObject SpawnPortal(Vector3 position)
        {
            GameObject gameObject = Object.Instantiate(Portal, new Vector3(position.x, position.y, Global.LayersPositions[Portal.tag]), Quaternion.identity);
            gameObject.name = Portal.name;
            Global.Debug("Spawn " + gameObject.name + " on position: " + position);
            return gameObject;
        }

        private static Sprite GetSpriteByPrefabName(string name)
        {
            if (name == TeleportDoor.name)
                return TeleportDoor;
            if (Sprites.Where(x => x.name == name).FirstOrDefault() == default)
                return Empty.GetComponent<SpriteRenderer>().sprite;
            return Sprites.Where(x => x.name == name).First();
        }

        public static GameObject SpawnBlock(Block block, Vector2 position, Vector2 roomPosition)
        {
            position = MapController.GetGlobalPosition(position, roomPosition);
            GameObject gameObject = Object.Instantiate(Empty, new Vector3(position.x, position.y, block.IsWall ? Global.LayersPositions[Global.WallsTag] : Global.LayersPositions[Global.BackgroundTag]), Quaternion.identity);
            gameObject.name = block.GameObjectPrefabName;
            gameObject.GetComponent<SpriteRenderer>().sprite = GetSpriteByPrefabName(gameObject.name);
            switch (block.BlockType)
            {
                case BlockType.DefaultBlock:
                    if (block.IsWall)
                        gameObject.AddComponent<BoxCollider2D>();
                    break;
                case BlockType.TeleportDoor:
                    gameObject.AddComponent<BoxCollider2D>();
                    gameObject.AddComponent<TeleportDoorComponent>();
                    gameObject.GetComponent<TeleportDoorComponent>().TeleportRoomId = block.TeleportRoomId;
                    break;
            }
            if (gameObject.GetComponent<TeleportDoorComponent>() != null)
                Global.Debug("Spawn door id: " + gameObject.GetComponent<TeleportDoorComponent>().TeleportRoomId + " on position: " + position);
            return gameObject;
        }
    }
}