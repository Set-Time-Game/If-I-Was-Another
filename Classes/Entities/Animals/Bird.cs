using System;
using System.Collections;
using Classes.Entities.Enemies;
using Classes.World;
using UnityEngine;
using static Classes.Utils.Flags;

namespace Classes.Entities.Animals
{
    public sealed class Bird : Animal
    {
        public SpawnerProp spawner;

        private void Awake()
        {
            onDieEvent += () =>
            {
                animator.SetTrigger(Death);
            };
            
            onTakeDamageEvent += (damage) =>
            {
                animator.SetTrigger(Damaged);
            };
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);

            if (!other.CompareTag("Player")) return;
            
            FlyAway();
        }

        private void FlyAway()
        {
            animator.SetTrigger(FlyUp);
        }

        private void FlyAwayEnd()
        {
            Destroy(GameObject);
            spawner.Respawn();
        }

        private void Die()
        {
            FlyAwayEnd();
        }
    }
}