using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Project
{
    public class PointerEventHandler : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private bool m_interactable = true;

        [Header("Events")]
        [SerializeField] private UnityEvent<PointerEventData> m_onLeftButtonClick;
        [SerializeField] private UnityEvent<PointerEventData> m_onRightButtonClick;

        public bool Interactable
        {
            get => m_interactable;
            set => m_interactable = value;
        }

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