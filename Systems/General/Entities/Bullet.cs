using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.General.Entities
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 1.0f)] private float m_speed;
        [SerializeField] private float m_lifetime;
        [SerializeField] private Rigidbody2D m_rigidbody2D;
        [SerializeField] private Transform m_transform;
    
        private IEnumerator Start()
        {
            m_rigidbody2D.AddForce(-m_transform.right * m_speed * Time.fixedDeltaTime, ForceMode2D.Impulse);

            yield return new WaitForSeconds(m_lifetime);
            
            Destroy(gameObject);
        }
    }
}
