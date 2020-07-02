using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Objects.Settings;
using Assets.Scripts.Manage;
using System.Linq;
using Assets.Scripts.Objects.Layout;

namespace Assets.Scripts.Objects.Room
{
    public class Room
    {
        private readonly System.Random Random = new System.Random();
        private List<Entity> EntitiesOnRoom = new List<Entity>();
        public List<NearbyPassRoom> NearbyPassRooms = new List<NearbyPassRoom>();
        public bool IsDoorsCreate { get; set; }
        public int Id { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Center { get; set; }
        public Block[,] BlocksOnRoom { get; set; }
        public GameObject[,] BlocksOnScene { get; set; }
        public Size Size { get; set; }
        public bool IsLoaded { get; set; }
        public bool IsBossesRoom { get; set; }
        public int EnemiesCount { get; set; }
        public bool IsLockedDoors { get; set; }
        public Sprite Sprite { get; private set; }
        public Room(int Id, Vector2 Position, Sprite Sprite)
        {
            this.Id = Id;
            this.Position = Position;
            this.Sprite = Sprite;

            Size = new Size(Random.Next(Layout.Layout.MinRoomSize, Layout.Layout.MaxRoomSize), Random.Next(Layout.Layout.MinRoomSize, Layout.Layout.MaxRoomSize));

            BlocksOnRoom = new Block[Size.Height, Size.Width];
            BlocksOnScene = new GameObject[Size.Height, Size.Width];

            for (int i = 0; i < BlocksOnRoom.GetLength(0); i++)
            {
                for (int k = 0; k < BlocksOnRoom.GetLength(1); k++)
                {
                    BlocksOnRoom[i, k] = new Block(Sprite.name, BlockType.DefaultBlock);
                    if (i == 0 || k == 0 || i == BlocksOnRoom.GetLength(0) - 1 || k == BlocksOnRoom.GetLength(1) - 1)
                    {
                        BlocksOnRoom[i, k].IsWall = true;
                    }
                }
            }
            Center = new Vector2(BlocksOnRoom.GetLength(0) / 2, BlocksOnRoom.GetLength(1) / 2);
        }


        public void Load()
        {
            LoadBlocks();
            LoadEnemies();
            IsLoaded = true;
        }
        public void Unload()
        {
            UnloadEntities();
            UnloadBlocks();
            IsLoaded = false;
        }
        public void CreateDoors()
        {
            foreach (NearbyPassRoom nearbyPassRoom in NearbyPassRooms)
            {
                switch (nearbyPassRoom.Direction)
                {
                    case Direction.Left:
                        BlocksOnRoom[0, BlocksOnRoom.GetLength(1) / 2] = new Block(Subjects.TeleportDoor.name, BlockType.TeleportDoor)
                        {
                            TeleportRoomId = nearbyPassRoom.Id
                        };
                        break;
                    case Direction.Right:
                        BlocksOnRoom[BlocksOnRoom.GetLength(0) - 1, BlocksOnRoom.GetLength(1) / 2] = new Block(Subjects.TeleportDoor.name, BlockType.TeleportDoor)
                        {
                            TeleportRoomId = nearbyPassRoom.Id
                        };
                        break;
                    case Direction.Down:
                        BlocksOnRoom[BlocksOnRoom.GetLength(0) / 2, 0] = new Block(Subjects.TeleportDoor.name, BlockType.TeleportDoor)
                        {
                            TeleportRoomId = nearbyPassRoom.Id
                        };
                        break;
                    case Direction.Up:
                        BlocksOnRoom[BlocksOnRoom.GetLength(0) / 2, BlocksOnRoom.GetLength(1) - 1] = new Block(Subjects.TeleportDoor.name, BlockType.TeleportDoor)
                        {
                            TeleportRoomId = nearbyPassRoom.Id
                        };
                        break;
                }
                Global.Debug("Create door: " + Id + " and " + nearbyPassRoom.Id + " with direction: " + nearbyPassRoom.Direction);
            }
            IsDoorsCreate = true;
        }
        public void CreateEnemies()
        {
            for (int i = 0; i < EnemiesCount; i++)
            {
                EntitiesOnRoom.Add(new Entity(Id, MapController.RoomPossibleEntities.GetByBoss(IsBossesRoom)[Random.Next(MapController.RoomPossibleEntities.GetByBoss(IsBossesRoom).Count)].name, IsBossesRoom, EntityType.Enemy, EntityStage.Alive));
                Global.Debug("Add entity: '" + EntitiesOnRoom.Last().GameObjectPrefabName + "'");
            }
        }  
        public void DeleteEnemies()
        {
            EntitiesOnRoom.Clear();
        }
        public void AddEntityToScene(Entity entity)
        {
            EntitiesOnRoom.Add(entity);
        }
        public void SetEntityAsCorpses(GameObject gameObject, GameObject corpse)
        {
            EntitiesOnRoom.Where(x => x.GameObjectOnScene == gameObject).FirstOrDefault().EntityStage = EntityStage.Dead;
            EntitiesOnRoom.Where(x => x.GameObjectOnScene == gameObject).FirstOrDefault().GameObjectOnScene = corpse;
        }
        public void RemoveEntityFromScene(GameObject gameObject)
        {
            EntitiesOnRoom.Remove(EntitiesOnRoom.Where(x => x.GameObjectOnScene == gameObject).FirstOrDefault());
        }
        public List<Entity> GetEntities()
        {
            return EntitiesOnRoom;
        }
        #region load
        private void LoadBlocks()
        {
            for (int i = 0; i < BlocksOnRoom.GetLength(0); i++)
            {
                for (int k = 0; k < BlocksOnRoom.GetLength(1); k++)
                {
                    BlocksOnScene[i, k] = Subjects.SpawnBlock(BlocksOnRoom[i, k], new Vector2(i, k), Position);
                }
            }
        }
        public void LoadEnemies()
        {
            foreach (Entity entity in EntitiesOnRoom)
            {
                entity.GameObjectOnScene = Creatures.SpawnEntity(entity, GetRandomEntityPos(), Position);
                entity.EntityStage = EntityStage.Alive;
            }
        }

        private Vector2 GetRandomEntityPos()
        {
            int distancefromwall = 2;
            return new Vector2(Random.Next(0 + distancefromwall, Size.Height - distancefromwall), Random.Next(0 + distancefromwall, Size.Width - distancefromwall));
        }

        #endregion

        #region unload
        private void UnloadBlocks()
        {
            foreach (GameObject gameObject in BlocksOnScene)
            {
                Object.Destroy(gameObject);
            }
        }
        public void UnloadEntities()
        {
            foreach (Entity entity in EntitiesOnRoom)
            {
                if (entity.EntityType == EntityType.Player)
                    continue;
                Object.Destroy(entity.GameObjectOnScene);
            }
            EntitiesOnRoom = EntitiesOnRoom.Where(x => x.EntityMode == EntityMode.Static).ToList();
        }
        #endregion

    }

}