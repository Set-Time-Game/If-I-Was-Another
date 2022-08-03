using System;
using System.Diagnostics;
using Types.Classes;
using UnityEngine;

namespace Systems.General.Control
{
    public class AttackPoint : MonoBehaviour
    {
        [SerializeField] private GameObject m_bulletPrefab;
        [SerializeField] private Player m_Player;
        [SerializeField] private ControlStick m_AttackStick;
        [SerializeField] private Transform m_Transform;
        [SerializeField] [Range(0.0f, 1.0f)] private float m_AttackRadius;
        [SerializeField] private Vector2 m_Offset;

        public void Attack(Vector2 direction)
        {
            AttackMove(direction);
        }

        private void AttackMelee()
        {
            //TODO: take enemies by map
        }
        
        public void AttackRange() 
            => Instantiate(m_bulletPrefab, m_Transform.position, m_Transform.rotation);

        public void SetAttack(AttackMode _, AttackMode mode)
        {
            if (!Enum.TryParse<AxisOptions>(((byte) mode).ToString(), out var newMode)) return;
            m_AttackStick.mode = newMode;
        }
        
        public void AttackMove(Vector2 direction)
        {
            if (direction.y == 0 && direction.x == 0) return;
            
            var isHorizontal = Math.Abs(direction.x) > Math.Abs(direction.y);
            var position = (Vector2) (m_Player.AttackMode switch
            {
                AttackMode.Melee => new Vector3
                (isHorizontal ? direction.x * m_Offset.x : 0,
                    !isHorizontal ? direction.y * m_Offset.y : 0, 0),

                AttackMode.Range => new Vector3
                (m_Offset.x * direction.x,
                    m_Offset.y * direction.y, 0),

                _ => m_Transform.localPosition
            });

            position += m_Player.capsuleCollider.offset;
            if (direction.y < 0)
                position.y = 0;

            m_Transform.localPosition = position;
            Rotate(m_Transform, m_Player.Transform);
        }
    
        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(m_Transform.position, m_AttackRadius);

        private void Rotate(Transform rotateTarget, Transform rotateToTarget)
        {
            var diff = (Vector2) rotateToTarget.position + m_Player.capsuleCollider.offset - (Vector2) rotateTarget.position;
            diff.Normalize();

            rotateTarget.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
        }
    }
}
