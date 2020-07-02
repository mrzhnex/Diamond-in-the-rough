using Assets.Scripts.Effects;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Objects.Layout;

namespace Assets.Scripts.Manage
{
    public static class Magic
    {
        public static GameObject Explosion;
        public static GameObject BlueStars;

        private static readonly float DistanceBetweenSpellAndCaster = 0.1f;
        private static readonly float DamageCount = 10.0f;

        public delegate void Spell(Vector2 position, GameObject Owner, params object[] args);

        public static readonly Dictionary<int, Spell> Spells = new Dictionary<int, Spell>()
        {
            {0, CreateBlueStarsProjectile },
            {1, CreateExplosion },
            {2, Swift },
            {3, CreateSpray }
        };

        public static void CreateBlueStarsProjectile(Vector2 position, GameObject owner, params object[] args)
        {
            Vector3 startPosition = new Vector3(position.x, position.y, Global.LayersPositions[BlueStars.tag]);
            Vector3 targetPosition = new Vector3(((Vector3)(Vector2)args[0]).x, ((Vector3)(Vector2)args[0]).y, Global.LayersPositions[BlueStars.tag]);
            GameObject fireLight = Object.Instantiate(BlueStars, startPosition + Global.GetDirection(startPosition, targetPosition) * DistanceBetweenSpellAndCaster, Quaternion.identity);
            fireLight.AddComponent<Projectile>();
            fireLight.GetComponent<Projectile>().SetOwner(owner);
            fireLight.GetComponent<Projectile>().Direction = Global.GetDirection(startPosition, targetPosition);
            fireLight.GetComponent<Projectile>().ProjectileSpeed = (float)args[1];
            if (MapController.LocationStage == LocationStage.Map)
            {
                MapController.CurrentLayout.CurrentRoom.AddEntityToScene(new Entity(MapController.CurrentLayout.CurrentRoom.Id, fireLight, EntityType.Neutral, EntityStage.Alive));
            }
        }

        public static void CreateExplosion(Vector2 position, GameObject owner, params object[] args)
        {
            GameObject gameObject = Object.Instantiate(Explosion, new Vector3(position.x, position.y, Global.LayersPositions[Explosion.tag]), Quaternion.identity);
            if (owner != null)
            {
                gameObject.tag = owner.tag;
            }
            else
            {
                gameObject.tag = Global.UntaggedTag;
            }
            gameObject.GetComponent<Explosion>().SetOwner(owner);
            if (args.Length > 0)
                gameObject.GetComponent<Explosion>().SetObjectOwner((GameObject)args[0]);
        }

        public static void Swift(Vector2 direction, GameObject owner, params object[] args)
        {
            direction = new Vector2(direction.x * (float)args[0], direction.y * (float)args[0]);
            if (!owner.GetComponent<Rigidbody2D>())
                owner.AddComponent<Rigidbody2D>();
            owner.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
        }

        public static void CreateSpray(Vector2 position, GameObject owner, params object[] args)
        {

        }

        public static void Damage(GameObject owner, GameObject target)
        {
            if (Global.GameStage == GameStage.InGame && MapController.LocationStage == LocationStage.Map && target != null && target.GetComponent<LifeForm>() != null)
            {
                target.GetComponent<LifeForm>().Damage(DamageCount);
                PopupManager.CreatePopup(new Vector2(target.transform.position.x, target.transform.position.y), DamageCount.ToString());
                if (owner == null || owner.GetComponent<LifeForm>() == null || owner.name == target.name)
                    return;
                owner.GetComponent<LifeForm>().Heal(DamageCount);
            }
        }
    }
}