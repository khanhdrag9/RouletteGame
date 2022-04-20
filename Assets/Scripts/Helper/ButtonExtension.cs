using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.Helper
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
