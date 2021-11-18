using System;
using Classes.World;
using Mobs.Enemy;
using Pathfinding;
using UnityEngine;
using static Classes.Utils.Flags;

namespace Classes.Entities.Enemies
{
    public abstract class Enemy : EntityAttackable
    {
        [Space]
        [SerializeField] public AIPath aiPath;
        [SerializeField] protected Seeker seeker;
        [SerializeField] protected AIDestinationSetter targetSetter;
        [SerializeField] protected Collider2D enemyCloseTrigger;
        [SerializeField] protected EnemyCollector collector;

        public string mobGenus;
        public Animator animator;
        public EnemySpawner spawner;
        protected Path Path;

        protected virtual void Awake()
        {
            onDieEvent +=
                () =>
                {
                    spawner.Spawn();
                    Destroy(GameObject);
                };
        }

        protected virtual void Start() =>
            onTakeDamageEvent +=
                damage =>
                {
                    ChangeState(States.Stunned);
                    animator.SetTrigger(Damaged);
                };

        public override void ChangeState(States state)
        {
            base.ChangeState(state);

            var trigger = -1;
            switch (state)
            {
                case States.None:
                    aiPath.canMove = true;
                    break;
                case States.Attacking:
                    trigger = Attack;
                    aiPath.canMove = false;
                    break;
                case States.Stunned:
                    aiPath.canMove = false;
                    break;
                case States.Sleep:
                    trigger = Sleep;
                    aiPath.canMove = false;
                    break;
            }

            if (trigger != -1)
                animator.SetTrigger(trigger);
        }
    }
}