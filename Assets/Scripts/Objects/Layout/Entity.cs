using UnityEngine;

namespace Assets.Scripts.Objects.Layout
{
    public class Entity
    {
        public int RoomId { get; private set; }
        public EntityMode EntityMode { get; set; }
        public EntityStage EntityStage { get; set; }
        public EntityType EntityType { get; set; }
        public GameObject GameObjectOnScene { get; set; }
        public string GameObjectPrefabName { get; private set; }
        public bool IsBoss { get; set; }
        public Entity (int RoomId, string GameObjectPrefabName, bool IsBoss, EntityType EntityType, EntityStage EntityStage)
        {
            EntityMode = EntityMode.Static;
            this.RoomId = RoomId;
            this.GameObjectPrefabName = GameObjectPrefabName;
            this.IsBoss = IsBoss;
            this.EntityType = EntityType;
            this.EntityStage = EntityStage;
        }
        public Entity(int RoomId, GameObject GameObjectOnScene, EntityType EntityType, EntityStage EntityStage)
        {
            EntityMode = EntityMode.Dynamic;
            GameObjectPrefabName = GameObjectOnScene.name;
            this.RoomId = RoomId;
            this.GameObjectOnScene = GameObjectOnScene;
            this.EntityType = EntityType;
            this.EntityStage = EntityStage;
        }

        public bool IsEnemy(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Enemy:
                    if (EntityType == EntityType.Player)
                        return true;
                    if (EntityType == EntityType.Neutral)
                        return true;
                    break;
                case EntityType.Player:
                    if (EntityType == EntityType.Enemy)
                        return true;
                    break;
            }
            return false;
        }
    }
    public enum EntityMode
    {
        Static, Dynamic
    }

    public enum EntityType
    {
        Enemy, Neutral, Player
    }
    public enum EntityStage
    {
        Alive, Dead
    }
}