using System;
using UnityEngine;
namespace TowerDefense.Runtime
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private Transform[] m_path = default;
        public int Length => m_path.Length;
        public Transform GetPoint(int index)
        {
            if (m_path == null || index >= m_path.Length)
            {
                throw new Exception("Failed to get point!");               
            }
            return m_path[index];
        }
        private void OnDrawGizmos()
        {
            if (m_path == null)
            {
                return;
            }
            for (int i = 0; i < m_path.Length - 1; i++)
            {
                Debug.DrawLine(m_path[i].position, m_path[i + 1].position);
            }
        }
    }
}
