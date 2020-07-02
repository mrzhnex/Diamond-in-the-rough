using Assets.Scripts.Manage;

namespace Assets.Scripts.Objects.Room
{
    public class Block
    {
        public BlockType BlockType { get; private set; }
        public string GameObjectPrefabName { get; private set; }
        public int TeleportRoomId { get; set; }
        public bool IsWall { get; set; }

        public Block(string GameObjectPrefabName, BlockType BlockType)
        {
            this.GameObjectPrefabName = GameObjectPrefabName;
            this.BlockType = BlockType;
        }
    }
}