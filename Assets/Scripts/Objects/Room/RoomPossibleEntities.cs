using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Objects.Room
{
    public class RoomPossibleEntities
    {
        public List<GameObject> DefaultEnemies { get; set; }
        public List<GameObject> BossesEnemies { get; set; }
        public RoomPossibleEntities(List<GameObject> DefaultEnemies, List<GameObject> BossesEnemies)
        {
            this.DefaultEnemies = DefaultEnemies;
            this.BossesEnemies = BossesEnemies;
        }
        public List<GameObject> GetByBoss(bool IsBoss)
        {
            if (IsBoss)
            {
                return BossesEnemies;
            }
            else
            {
                return DefaultEnemies;
            }
        }
    }
}