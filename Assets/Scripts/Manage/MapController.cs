using Assets.Scripts.Objects.Room;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Objects.Layout;
using System.Linq;
using Assets.Scripts.Objects.Hideout;
using Assets.Scripts.Main;

namespace Assets.Scripts.Manage
{
    public static class MapController
    {
        public static Layout CurrentLayout { get; set; }
        public static RoomPossibleEntities RoomPossibleEntities { get; set; }

        #region newgame
        public static void GenerateAndStartLayout()
        {
            if (CurrentLayout != null)
                CurrentLayout.Unload();

            if (Global.PlayerOnScene.GetComponent<GenerateLayoutAsyncComponent>())
                Object.Destroy(Global.PlayerOnScene.GetComponent<GenerateLayoutAsyncComponent>());
            if (Global.PlayerOnScene.GetComponent<LoadLayoutAsyncComponent>())
                Object.Destroy(Global.PlayerOnScene.GetComponent<LoadLayoutAsyncComponent>());
            if (Global.PlayerOnScene.GetComponent<ProcessLayoutAsyncComponent>())
                Object.Destroy(Global.PlayerOnScene.GetComponent<ProcessLayoutAsyncComponent>());

            Global.PlayerOnScene.AddComponent<GenerateLayoutAsyncComponent>();
            Global.PlayerOnScene.AddComponent<LoadLayoutAsyncComponent>();
        }

        public static LocationStage LocationStage = LocationStage.Other;

        public static void Respawn()
        {
            Global.Debug("Respawn player...");
            foreach (Room room in CurrentLayout.Rooms)
            {
                room.UnloadEntities();
                room.LoadEnemies();
            }
            StartRoom(CurrentLayout.PlayerStartRoom, CurrentLayout.PlayerStartRoom.Center);
            Global.PlayerOnScene.GetComponent<LifeForm>().Heal(Global.PlayerOnScene.GetComponent<LifeForm>().MaxHealth);
        }

        public static void LoadHideout()
        {
            if (CurrentLayout != null)
                CurrentLayout.Unload();
            if (Global.PlayerOnScene.GetComponent<GenerateLayoutAsyncComponent>())
                Object.Destroy(Global.PlayerOnScene.GetComponent<GenerateLayoutAsyncComponent>());
            if (Global.PlayerOnScene.GetComponent<LoadLayoutAsyncComponent>())
                Object.Destroy(Global.PlayerOnScene.GetComponent<LoadLayoutAsyncComponent>());
            if (Global.PlayerOnScene.GetComponent<ProcessLayoutAsyncComponent>())
                Object.Destroy(Global.PlayerOnScene.GetComponent<ProcessLayoutAsyncComponent>());
            Global.PlayerOnScene.transform.position = new Vector3(Global.HideoutOnScene.GetComponentsInChildren<Transform>().Where(x => x.gameObject.name == "Start").First().position.x, Global.HideoutOnScene.GetComponentsInChildren<Transform>().Where(x => x.gameObject.name == "Start").First().position.x, Global.LayersPositions[Global.PlayerOnScene.tag]);
            StartupComponent.LoadHideout();
        }
        #endregion

        public static Vector2 GetGlobalPosition(Vector2 position, Vector2 roomPosition)
        {
            return new Vector2(position.x + Layout.MaxRoomSize * 2 * roomPosition.y, position.y + Layout.MaxRoomSize * 2 * roomPosition.x);
        }

        #region reloadroom
        public static void SetInVulnerability(GameObject gameObject)
        {
            if (gameObject.GetComponent<InVulnerability>())
                Object.Destroy(gameObject.GetComponent<InVulnerability>());
            else
                gameObject.AddComponent<InVulnerability>();
        }

        public static void LockTeleportDoors(Room room)
        {
            room.IsLockedDoors = true;
        }
        public static void UnlockTeleportDoors(Room room)
        {
            room.IsLockedDoors = false;
        }


        public static void StartRoom(Room room, Vector2 playerStartPosition)
        {
            playerStartPosition = GetGlobalPosition(playerStartPosition, room.Position);
            Global.PlayerOnScene.transform.position = new Vector3(playerStartPosition.x, playerStartPosition.y, Global.LayersPositions[Global.PlayerOnScene.tag]);
            if (CurrentLayout.CurrentRoom != null)
            {
                CurrentLayout.CurrentRoom.RemoveEntityFromScene(Global.PlayerOnScene);
            }
            CurrentLayout.CurrentRoom = room;
            if (CurrentLayout.CurrentRoom.GetEntities().Where(x => x.EntityType == EntityType.Enemy && x.EntityStage == EntityStage.Alive).ToList().Count > 0)
                SetInVulnerability(Global.PlayerOnScene);
            CurrentLayout.CurrentRoom.AddEntityToScene(new Entity(CurrentLayout.CurrentRoom.Id, Global.PlayerOnScene, EntityType.Player, EntityStage.Alive));
            Global.SetGameStage(GameStage.InGame);
            LocationStage = LocationStage.Map;
            Global.Debug("Load room: " + room.Id + " with start position: " + playerStartPosition);
        }

        public static void LoadRoomById(int roomId)
        {
            Global.Debug("Start load new room: " + roomId);
            Room room = CurrentLayout.GetRoomById(roomId);
            Vector2 playerStartPosition = new Vector2(-1000, -1000);
            foreach (NearbyPassRoom nearbyPassRoom in room.NearbyPassRooms)
            {
                if (nearbyPassRoom.Id == CurrentLayout.CurrentRoom.Id)
                {
                    switch (nearbyPassRoom.Direction)
                    {
                        case Direction.Left:
                            playerStartPosition = new Vector2(0, room.BlocksOnRoom.GetLength(1) / 2) + Vector2.right;
                            break;
                        case Direction.Right:
                            playerStartPosition = new Vector2(room.BlocksOnRoom.GetLength(0) - 1, room.BlocksOnRoom.GetLength(1) / 2) + Vector2.left;
                            break;
                        case Direction.Down:
                            playerStartPosition = new Vector2(room.BlocksOnRoom.GetLength(0) / 2, 0) + Vector2.up;
                            break;
                        case Direction.Up:
                            playerStartPosition = new Vector2(room.BlocksOnRoom.GetLength(0) / 2, room.BlocksOnRoom.GetLength(1) - 1) + Vector2.down;
                            break;
                    }
                    break;
                }
            }
            StartRoom(room, playerStartPosition);
        }

        #endregion

        #region helper

        public static readonly Dictionary<Direction, Direction> RevertDirections = new Dictionary<Direction, Direction>()
        {
            {Direction.Down, Direction.Up },
            {Direction.Up, Direction.Down },
            {Direction.Right, Direction.Left },
            {Direction.Left, Direction.Right }
        };
        #endregion
    }

    public enum LocationStage
    {
        Hideout, Map, Other
    }
    public enum Direction
    {
        Left, Right, Down, Up, None
    }
    public enum BlockType
    {
        TeleportDoor, DefaultBlock
    }
}