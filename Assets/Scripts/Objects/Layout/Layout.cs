using UnityEngine;
using System;
using Assets.Scripts.Manage;
using System.Collections.Generic;
using Assets.Scripts.Objects.Room;
using System.Linq;

namespace Assets.Scripts.Objects.Layout
{
    public class Layout
    {
        private readonly System.Random Random = new System.Random();
        public Room.Room CurrentRoom { get; set; }
        public Room.Room PlayerStartRoom { get; set; }
        public int EnemyCount { get; set; }
        public Room.Room[,] Rooms { get; set; }
        public static int MinRoomSize = 10;
        public static int MaxRoomSize = 20;
        public Layout(int PlayerLevel)
        {
            Rooms = new Room.Room[PlayerLevel / 10 + 3, PlayerLevel / 10 + 3];
            int id = 0;
            for (int i = 0; i < Rooms.GetLength(0); i++)
            {
                for (int k = 0; k < Rooms.GetLength(1); k++)
                {
                    Rooms[i, k] = new Room.Room(id, new Vector2(i, k), Subjects.Sprites[Random.Next(Subjects.Sprites.Length)]);
                    if (Random.Next(-PlayerLevel, PlayerLevel) > i)
                    {
                        EnemyCount += i;
                    }
                    id++;
                    Rooms[i, k].EnemiesCount = 1;
                    Rooms[i, k].CreateEnemies();

                    Global.Debug("Create room: " + Rooms[i, k].Id + " sprite: " + Rooms[i, k].Sprite + " position: " + Rooms[i, k].Position + " size: " + Rooms[i, k].Size.ToString());
                }
            }

            for (int i = 0; i < Rooms.GetLength(0); i++)
            {
                for (int k = 0; k < Rooms.GetLength(1); k++)
                {
                    switch (Random.Next(0, 4))
                    {
                        case 0:
                            if (i != 0)
                            {
                                if (!IsRoomConnected(Rooms[i, k], Rooms[i - 1, k]) && IsNearbyRoom(Rooms[i, k], Rooms[i - 1, k], out Direction direction))
                                {
                                    MakeNearby(Rooms[i, k], Rooms[i - 1, k], direction);
                                }
                            }
                            break;
                        case 1:
                            if (i != Rooms.GetLength(0) - 1)
                            {
                                if (!IsRoomConnected(Rooms[i, k], Rooms[i + 1, k]) && IsNearbyRoom(Rooms[i, k], Rooms[i + 1, k], out Direction direction))
                                {
                                    MakeNearby(Rooms[i, k], Rooms[i + 1, k], direction);
                                }
                            }
                            break;
                        case 2:
                            if (k != 0)
                            {
                                if (!IsRoomConnected(Rooms[i, k], Rooms[i, k - 1]) && IsNearbyRoom(Rooms[i, k], Rooms[i, k - 1], out Direction direction))
                                {
                                    MakeNearby(Rooms[i, k], Rooms[i, k - 1], direction);
                                }
                            }
                            break;
                        case 3:
                            if (k != Rooms.GetLength(1) - 1)
                            {
                                if (!IsRoomConnected(Rooms[i, k], Rooms[i, k + 1]) && IsNearbyRoom(Rooms[i, k], Rooms[i, k + 1], out Direction direction))
                                {
                                    MakeNearby(Rooms[i, k], Rooms[i, k + 1], direction);
                                }
                            }
                            break;
                    }

                }
            }

            if (Rooms.ToList().Where(x => x.EnemiesCount == 0).FirstOrDefault() != default)
            {
                PlayerStartRoom = Rooms.ToList().Where(x => x.EnemiesCount == 0).FirstOrDefault();
            }
            else
            {
                PlayerStartRoom = Rooms[Random.Next(Rooms.GetLength(0)), Random.Next(Rooms.GetLength(1))];
                PlayerStartRoom.DeleteEnemies();
            }
            CurrentRoom = PlayerStartRoom;
            Global.Debug("Set start room: " + PlayerStartRoom.Id + " start position: " + PlayerStartRoom.Center);
        }

        public Room.Room GetRoomByGameobject(GameObject target)
        {
            foreach (Room.Room room in Rooms)
            {
                foreach (Entity entity in room.GetEntities())
                {
                    if (entity.GameObjectOnScene == target)
                        return room;
                }
            }
            return null;
        }


        public Room.Room GetRoomById(int roomId)
        {
            for (int i = 0; i < Rooms.GetLength(0); i++)
            {
                for (int k = 0; k < Rooms.GetLength(1); k++)
                {
                    if (Rooms[i, k].Id == roomId)
                    {
                        return Rooms[i, k];
                    }
                }
            }
            return null;
        }

        public List<List<Room.Room>> GetKlasters(out int count)
        {
            List<List<Room.Room>> Klasters = new List<List<Room.Room>>();

            for (int i = 0; i < Rooms.GetLength(0); i++)
            {
                for (int k = 0; k < Rooms.GetLength(1); k++)
                {
                    bool set = false;
                    foreach (List<Room.Room> klaster in Klasters)
                    {
                        foreach (Room.Room room in klaster)
                        {
                            if (RoomsPossibleWay(room, Rooms[i, k], new List<int>()))
                            {
                                klaster.Add(Rooms[i, k]);
                                set = true;
                                break;
                            }
                        }
                        if (set)
                        {
                            break;
                        }
                    }
                    if (!set)
                    {
                        List<Room.Room> klaster = new List<Room.Room>
                        {
                            Rooms[i, k]
                        };
                        Klasters.Add(klaster);
                    }
                }
            }

            count = Klasters.Count;
            Global.Debug("Klasters count: " + count);
            return Klasters;
        }

        public void SetKlasters(List<List<Room.Room>> Klasters)
        {
            int startI = 0;
            int endI = Klasters.Count;
            int startL = 0;
            int endL = Klasters.Count;
            switch (Random.Next(0, 4))
            {
                case 0:
                    startI = Random.Next(startI, endI);
                    break;
                case 1:
                    startL = Random.Next(startL, endL);
                    break;
                case 2:
                    endI = Random.Next(startI, endI);
                    break;
                case 3:
                    endL = Random.Next(startL, endL);
                    break;
            }
            for (int i = startI; i < endI; i++)
            {
                for (int k = 0; k < Klasters[i].Count; k++)
                {

                    for (int l = startL; l < endL; l++)
                    {
                        for (int m = 0; m < Klasters[l].Count; m++)
                        {
                            if (i == l)
                            {
                                break;
                            }

                            if (IsNearbyRoom(Klasters[i][k], Klasters[l][m], out Direction direction))
                            {
                                MakeNearby(Klasters[i][k], Klasters[l][m], direction);
                                return;
                            }

                        }
                    }

                }
            }
        }

        private bool RoomsPossibleWay(Room.Room room, Room.Room nonNear, List<int> parentIdRooms)
        {
            foreach (NearbyPassRoom nearbyPassRoom in room.NearbyPassRooms)
            {
                if (parentIdRooms.Contains(nearbyPassRoom.Id))
                {
                    continue;
                }
                if (nearbyPassRoom.Id == nonNear.Id)
                {
                    return true;
                }
                if (IsRoomConnected(GetRoomById(nearbyPassRoom.Id), nonNear))
                {
                    return true;
                }
                else
                {
                    parentIdRooms.Add(nearbyPassRoom.Id);
                    if (RoomsPossibleWay(GetRoomById(nearbyPassRoom.Id), nonNear, parentIdRooms))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsNearbyRoom(Room.Room room, Room.Room near, out Direction direction)
        {
            for (int i = 0; i < Rooms.GetLength(0); i++)
            {
                for (int k = 0; k < Rooms.GetLength(1); k++)
                {
                    if (Rooms[i, k].Id == room.Id)
                    {
                        if (i != 0)
                        {
                            if (Rooms[i - 1, k].Id == near.Id)
                            {
                                direction = Direction.Down;
                                return true;
                            }
                        }
                        if (i != Rooms.GetLength(0) - 1)
                        {
                            if (Rooms[i + 1, k].Id == near.Id)
                            {
                                direction = Direction.Up;
                                return true;
                            }
                        }
                        if (k != 0)
                        {
                            if (Rooms[i, k - 1].Id == near.Id)
                            {
                                direction = Direction.Left;
                                return true;
                            }
                        }
                        if (k != Rooms.GetLength(1) - 1)
                        {
                            if (Rooms[i, k + 1].Id == near.Id)
                            {
                                direction = Direction.Right;
                                return true;
                            }
                        }
                    }
                }
            }
            direction = Direction.None;
            return false;
        }

        private void MakeNearby(Room.Room room, Room.Room near, Direction direction)
        {
            room.NearbyPassRooms.Add(new NearbyPassRoom() { Id = near.Id, Direction = direction });
            near.NearbyPassRooms.Add(new NearbyPassRoom() { Id = room.Id, Direction = MapController.RevertDirections[direction] });
            Global.Debug("Make nearby: " + room.Id + " and " + near.Id + " with direction: " + direction);
        }

        private bool IsRoomConnected(Room.Room room, Room.Room near)
        {
            foreach (NearbyPassRoom nearbyPassRoom in room.NearbyPassRooms)
            {
                if (nearbyPassRoom.Id == near.Id)
                {
                    return true;
                }
            }
            return false;
        }

        public void Unload()
        {
            for (int i = 0; i < Rooms.GetLength(0); i++)
            {
                for (int k = 0; k < Rooms.GetLength(1); k++)
                {
                    Rooms[i, k].Unload();
                }
            }
            CurrentRoom = null;
            PlayerStartRoom = null;
            EnemyCount = 0;
            Rooms = new Room.Room[0, 0];
        }

        public void Load()
        {
            for (int i = 0; i < Rooms.GetLength(0); i++)
            {
                for (int k = 0; k < Rooms.GetLength(1); k++)
                {
                    Rooms[i, k].Load();
                }
            }
        }
    }
}