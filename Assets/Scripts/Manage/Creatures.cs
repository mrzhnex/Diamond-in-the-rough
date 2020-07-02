using Assets.Scripts.Enemy;
using Assets.Scripts.Objects.Layout;
using UnityEngine;

namespace Assets.Scripts.Manage
{
    public static class Creatures
    {
        public static GameObject Laborant;
        public static GameObject ForkLift;
        public static GameObject Whell;
        public static GameObject Corpse;
        private static readonly float DistanceBetweenSpellAndCaster = 0.1f;
        private static System.Random Random = new System.Random();
        private static GameObject GetGameObjectByPrefabName(string name)
        {
            switch (name)
            {
                case nameof(Laborant):
                    return Laborant;
                case nameof(ForkLift):
                    return ForkLift;
                case nameof(Whell):
                    return Whell;
                case nameof(Corpse):
                    return Corpse;
                default:
                    Global.Debug("Null creature: '" + name + "'");
                    return Corpse;
            }
        }

        public static GameObject CreateFork(Vector2 parentPosition, Vector2 position, GameObject owner)
        {
            GameObject gameObject = Object.Instantiate(GetGameObjectByPrefabName(ForkLift.name), new Vector3((parentPosition + (Global.GetDirection(parentPosition, position) * DistanceBetweenSpellAndCaster)).x, (parentPosition + (Global.GetDirection(parentPosition, position) * DistanceBetweenSpellAndCaster)).y, Global.LayersPositions[ForkLift.tag]), Quaternion.identity);
            gameObject.name = ForkLift.name;
            gameObject.GetComponent<ForkBehaviour>().SetOwner(owner);
            return gameObject;
        }

        public static GameObject SpawnEntity(Entity entity, Vector2 position, Vector2 roomPosition)
        {
            position = MapController.GetGlobalPosition(position, roomPosition);
            GameObject gameObject = Object.Instantiate(GetGameObjectByPrefabName(entity.GameObjectPrefabName), new Vector3(position.x, position.y, Global.LayersPositions[GetGameObjectByPrefabName(entity.GameObjectPrefabName).tag]), Quaternion.identity);
            gameObject.name = entity.GameObjectPrefabName;
            return gameObject;
        }

        public static GameObject SpawnCorpse(Vector2 position, Sprite deathSprite)
        {
            GameObject gameObject = Object.Instantiate(Corpse, new Vector3(position.x, position.y, Global.LayersPositions[Corpse.tag]), Quaternion.Euler(0.0f, 0.0f, Random.Next(-180, 180)));
            gameObject.GetComponent<SpriteRenderer>().sprite = deathSprite;
            return gameObject;
        }

    }
}