using UnityEngine;

namespace Game.Helper
{
    public class AutoCenterGUI : MonoBehaviour
    {
        [SerializeField] private bool activeOnAwake = false;
        void Awake()
        {
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            gameObject.SetActive(activeOnAwake);
        }
    }
}
