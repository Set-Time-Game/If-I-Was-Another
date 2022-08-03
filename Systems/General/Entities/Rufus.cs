using System;
using System.Collections;
using Pathfinding;
using Systems.General.Control;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems.General.Entities
{
    public class Rufus : MonoBehaviour
    {
        [SerializeField] private Transform m_transform;
        [SerializeField] private Transform m_target;
        [SerializeField] private AIDestinationSetter targetSetter;

        [SerializeField] private Animator m_animator;
        [SerializeField] private SpriteRenderer m_spriteRenderer;

        private readonly WaitForSeconds m_timer = new(1);
        private readonly int X = Animator.StringToHash("X");
        private readonly int Y = Animator.StringToHash("Y");

        private IEnumerator Start()
        {
            while (true)
            {
                var direction = new Vector3(Random.Range(-1, 1) / 2f, Random.Range(-1, 1) / 2f, 0);
                m_target.localPosition = direction;
                var anim = ControlStick.SnapInput(direction, AxisOptions.Fixed);
                FlipSprite(anim);
                m_animator.SetFloat(X, anim.x);
                m_animator.SetFloat(Y, anim.y);

                yield return m_timer;
            }
        }

        private void Update()
        {
            var pos = m_transform.position;
            pos.z = 0;
            m_transform.position = pos;
        }
        
        private void FlipSprite(Vector2 position) 
            => FlipSprite(position.x < -0.7f);

        private void FlipSprite(bool state) 
            => m_spriteRenderer.flipX = state;
    }
}
