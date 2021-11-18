using System.Collections.Generic;
using System.Diagnostics;
using Classes.Utils;
using Pathfinding;
using UnityEngine;
using static Classes.Utils.Flags;

namespace Mobs.Enemy
{
    public sealed class EnemyCollector : Collector
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Classes.Entities.Enemies.Enemy parent;
        [SerializeField] private Transform defaultTarget;
        [SerializeField] private AIDestinationSetter targetSetter;
        
        public LayerMask enemyLayer;

        private void Start()
        {
            parent.onAttackingEvent += Attack;
            parent.onStateChangeEvent += (from, to) =>
            {
                if (to != States.Attacking) return;
                
                defaultTarget.position = targetSetter.target.position;
                targetSetter.target = defaultTarget;
            };
        }

        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(defaultTarget.position, parent.AttackRadius);
        
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Classes.Entities.Enemies.Enemy>(out var enemy) && enemy.mobGenus == parent.mobGenus) return;
            
            var before = Enemies.Count;
            base.OnTriggerEnter2D(collision);
            if (before == 0 && Enemies.Count > 0)
                parent.animator.SetTrigger(Awake);
        }
        
        private void Attack(float damage)
        {
            var targetPosition = defaultTarget.localPosition;
            Utils.Flip(spriteRenderer, targetPosition, parent.animator);
        }
        
        private void LateUpdate()
        {
            if (parent.State != States.None || !parent.aiPath.canMove) return;
            
            Utils.Flip(spriteRenderer, defaultTarget.localPosition, parent.animator);
            
            if (Enemies.Count < 1) return;

            var target = Utils.ClosestFrom(Enemies, transform.position)?.Transform;
            target ??= defaultTarget;

            targetSetter.target = target;
            defaultTarget.position = target.position;

            if (Vector2.Distance(transform.position, target.position) <= parent.AttackDistance)
                parent.ChangeState(States.Attacking);
            else
                parent.animator.SetFloat(Speed, 1);
        }
    }
}