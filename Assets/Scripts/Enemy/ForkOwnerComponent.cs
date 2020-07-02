using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class ForkOwnerComponent : MonoBehaviour
    {
        public float TimeToAttack = 1.0f;
        public int MaxForkCount = 1;
        [HideInInspector]
        public int ForkCount = 0;
    }
}