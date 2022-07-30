using System;
using System.Collections;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems.General.Entities
{
    public class Rufus : MonoBehaviour
    {
        [SerializeField] private Transform Transform;
        [SerializeField] private Transform Target;
        [SerializeField] private AIDestinationSetter targetSetter;

        [SerializeField] private Animator animator;

        private readonly WaitForSeconds m_timer = new(1);
        private readonly int X = Animator.StringToHash("X");
        private readonly int Y = Animator.StringToHash("Y");

        private IEnumerator Start()
        {
            while (true)
            {
                var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                Target.position += direction;
                animator.SetFloat(X, direction.x);
                animator.SetFloat(Y, direction.y);

                yield return m_timer;
            }
        }
    }
}
