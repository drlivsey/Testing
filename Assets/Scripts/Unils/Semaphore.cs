using UnityEngine;
using UnityEngine.Events;

namespace Project
{
    public class Semaphore : MonoBehaviour
    {
        [SerializeField] private bool m_checked = false;
        
        [Header("Events")]
        [SerializeField] private UnityEvent<bool> m_stateChanged;
        [SerializeField] private UnityEvent m_onChecked;
        [SerializeField] private UnityEvent m_onUnchecked;

        public bool Checked => m_checked;

        public void SwitchActivity()
        {
            m_checked = !m_checked;
            
            if (m_checked) m_onChecked?.Invoke();
            else m_onUnchecked?.Invoke();

            m_stateChanged?.Invoke(m_checked);
        }
    }
}