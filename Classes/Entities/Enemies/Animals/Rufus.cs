using System;
using System.Collections.Generic;
using System.Diagnostics;
using Classes.Utils;
using Pathfinding;
using UnityEngine;
using static Classes.Utils.Flags;

namespace Classes.Entities.Enemies.Animals
{
    public sealed class Rufus : EnemyAnimal
    {
        protected override void Awake()
        {
            base.Awake();
            onAttackingEvent += Attack;
            onStateChangeEvent += (from, to) =>
            {
                switch (to)
                {
                    case States.None:
                        animator.SetFloat(Speed, collector.Enemies.Count < 1 ? 0 : 1);
                        break;
                    case States.Attacking:
                        animator.SetFloat(Speed, 1);
                        break;
                    case States.Rolling:
                        break;
                    case States.Stunned:
                        break;
                    case States.Collecting:
                        break;
                    case States.Sleep:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(to), to, null);
                }
            };
        }
        
        private void Attack(float damage)
        {
            var hitEnemies = new Collider2D[collector.Enemies.Count + 1];
            if (Physics2D.OverlapCircleNonAlloc(
                //defaultTarget.position,
                targetSetter.target.position,
                AttackRadius,
                hitEnemies,
                collector.enemyLayer) < 1) return;

            var targets = new LinkedList<IDamageable>();
            foreach (var enemy in hitEnemies)
                if (enemy != null
                    && enemy.TryGetComponent<IDamageable>(out var damageable)
                    && !enemy.TryGetComponent<Enemy>(out _)
                    && !targets.Contains(damageable))
                    targets.AddLast(damageable);

            if (targets.Count < 1) return;
            Utils.Utils.ClosestFrom(targets, transform.position).TakeDamage(damage);
        }
    }
}