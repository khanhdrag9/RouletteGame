using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class ButtonExtension : MonoBehaviour, IPointerClickHandler 
    {
        public PointerEvent OnClick {get; } = new PointerEvent();
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick.Invoke(eventData);
        }

        public class PointerEvent : UnityEvent<PointerEventData> {}
    }
}
