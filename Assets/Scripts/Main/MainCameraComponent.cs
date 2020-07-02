using Assets.Scripts.Manage;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Main
{
    public class MainCameraComponent : MonoBehaviour
    {
        private Vector3 Difference;

        public void Start()
        {
            if (FindObjectOfType<PlayerController>())
            {
                transform.position = new Vector3(Global.PlayerOnScene.transform.position.x, Global.PlayerOnScene.transform.position.y, transform.position.z);
                Difference = transform.position - Global.PlayerOnScene.transform.position;
            }
            else
            {
                Global.Debug("PlayerController not found, destroy MainCameraComponent");
                Destroy(gameObject.GetComponent<MainCameraComponent>());
            }
        }

        public void Update()
        {
            if (Global.NonFreezingGameStages.Contains(Global.GameStage))
            {
                transform.position = Global.PlayerOnScene.transform.position + Difference;
            }
        }
    }
}