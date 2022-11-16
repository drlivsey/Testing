using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Project
{
    public class PointerEventHandler : MonoBehaviour, IPointerClickHandler
    {
        [Header("Events")]
        [SerializeField] private UnityEvent<PointerEventData> m_onLeftButtonClick;
        [SerializeField] private UnityEvent<PointerEventData> m_onRightButtonClick;

        public UnityEvent<PointerEventData> OnLeftButtonClick => m_onLeftButtonClick;
        public UnityEvent<PointerEventData> OnRightButtonClick => m_onRightButtonClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                OnLeftButtonClick?.Invoke(eventData);
            else if (eventData.button == PointerEventData.InputButton.Right)
                OnRightButtonClick?.Invoke(eventData);
        }
    }
}