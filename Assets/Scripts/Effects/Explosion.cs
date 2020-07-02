using Assets.Scripts.Manage;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class Explosion : MonoBehaviour
    {
        private GameObject Owner;
        private Animator Animator;
        private bool IsPlaying = true;
        private GameObject ObjectOwner;
        private string ObjectOwnerName = string.Empty;
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (ObjectOwner == null)
            {
                if (ObjectOwnerName != collision.gameObject.name)
                    Magic.Damage(Owner, collision.gameObject);
            }
            else
            {
                if (ObjectOwner.name != collision.gameObject.name)
                {
                    Magic.Damage(Owner, collision.gameObject);
                }
            }
        }

        public void Start()
        {
            Animator = gameObject.GetComponent<Animator>();
        }

        public void Update()
        {
            if (!Global.NonFreezingGameStages.Contains(Global.GameStage) && IsPlaying)
            {
                Global.Debug("Stop playing animation in: " + gameObject.name);
                IsPlaying = false;
                Animator.StartPlayback();
            }
            else if (Global.NonFreezingGameStages.Contains(Global.GameStage) && !IsPlaying)
            {
                Global.Debug("Start playing animation in: " + gameObject.name);
                IsPlaying = true;
                Animator.StopPlayback();
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void SetObjectOwner(GameObject ObjectOwner)
        {
            ObjectOwnerName = ObjectOwner.name;
            this.ObjectOwner = ObjectOwner;
        }

        public void SetOwner(GameObject Owner)
        {
            this.Owner = Owner;
        }
    }
}