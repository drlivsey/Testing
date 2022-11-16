using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Project
{
    [RequireComponent(typeof(TMP_InputField))]
    public class FloatFieldAgregator : MonoBehaviour
    {
        [SerializeField] private TMP_InputField m_inputField;

        [Header("Events")]
        [SerializeField] private UnityEvent<float> m_onValueChanged;

        private void Awake()
        {
            if (!m_inputField)
            {
                m_inputField = GetComponent<TMP_InputField>();
                m_inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
            }
        }

        private void OnEnable()
        {
            m_inputField.onValueChanged.AddListener(OnTextChanged);
        }

        private void OnDisable()
        {
            m_inputField.onValueChanged.RemoveListener(OnTextChanged);
        }

        private void OnTextChanged(string text)
        {
            if (float.TryParse(m_inputField.text, out var value))
            {
                m_onValueChanged?.Invoke(value);
            }
        }
    }
}