using Assets.Scripts.Effects;
using UnityEngine;

namespace Assets.Scripts.Manage
{
    public static class PopupManager
    {
        public static GameObject Popup;

        public static GameObject CreatePopup(Vector2 position, string text = "")
        {
            GameObject gameObject = Object.Instantiate(Popup, new Vector3(position.x, position.y, Global.LayersPositions[Popup.tag]), Quaternion.identity);
            gameObject.GetComponent<PopupComponent>().Setup(text);
            return gameObject;
        }
    }
}