using UnityEngine;

namespace Game.Helper
{
    public class AutoCenterGUI : MonoBehaviour
    {
        void Awake()
        {
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}
