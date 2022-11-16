using UnityEngine;

namespace Project.Coords
{
    public class Viewport : MonoBehaviour
    {
        [SerializeField] private Axis m_xAxis;
        [SerializeField] private Axis m_yAxis;

        private void Awake() 
        {
            InitializeAxis();
        }

        public void InitializeAxis()
        {
            
        }
    }
}