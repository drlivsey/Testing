using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class CollisionDetector : MonoBehaviour
{
    [SerializeField] private UnityEvent<GameObject> m_onCollisionEnter;
    [SerializeField] private UnityEvent<GameObject> m_onTriggerEnter;

    private void OnCollisionEnter(Collision other)
    {
        m_onCollisionEnter?.Invoke(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        m_onTriggerEnter?.Invoke(other.gameObject);
    }
}
