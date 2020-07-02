using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Effects
{
    [RequireComponent(typeof(Light))]
    public class Flicker : MonoBehaviour
    {
        public float MaxReduction;
        public float MaxIncrease;
        public float RateDamping;
        public float Strength;
        public bool StopFlickering;

        private Light lightSource;
        private float baseIntensity;
        private bool flickering;

        public void Reset()
        {
            MaxReduction = 0.2f;
            MaxIncrease = 0.2f;
            RateDamping = 0.1f;
            Strength = 300;
        }

        public void Start()
        {
            lightSource = GetComponent<Light>();
            baseIntensity = lightSource.intensity;
            StartCoroutine(DoFlicker());
        }

        void Update()
        {
            if (!StopFlickering && !flickering)
            {
                StartCoroutine(DoFlicker());
            }
        }

        private IEnumerator DoFlicker()
        {
            flickering = true;
            while (!StopFlickering)
            {
                lightSource.intensity = Mathf.Lerp(lightSource.intensity, Random.Range(baseIntensity - MaxReduction, baseIntensity + MaxIncrease), Strength * Time.deltaTime);
                yield return new WaitForSeconds(RateDamping);
            }
            flickering = false;
        }
    }
}