using TMPro;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class PopupComponent : MonoBehaviour
    {
        private TextMeshPro TextMesh;
        private Color Color;
        private float DisappearTimer = 0.7f;
        private readonly float MoveUpSpeed = 0.8f;
        private readonly float SizeReduced = 0.97f;

        public void Awake()
        {
            TextMesh = GetComponent<TextMeshPro>();
            Color = TextMesh.color;
        }

        public void Setup(string text)
        {
            TextMesh.SetText(text);
        }

        public void Update()
        {
            DisappearTimer -= Time.deltaTime;
            TextMesh.fontSize *= SizeReduced;
            if (DisappearTimer < 0)
            {
                Color.a -= 3.0f * Time.deltaTime;
                TextMesh.color = Color;
                if (Color.a < 0)
                {
                    Destroy(gameObject);
                }
            }
            transform.position += new Vector3(0, MoveUpSpeed) * Time.deltaTime;
        }
    }
}