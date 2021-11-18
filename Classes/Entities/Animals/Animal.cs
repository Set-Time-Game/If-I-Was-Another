using Classes.World;
using UnityEngine;
using static Classes.Utils.Flags;

namespace Classes.Entities.Animals
{
    public abstract class Animal : EntityDamageable
    {
        //[SerializeField] protected Collider2D enemyCloseTrigger;
        [Space]
        [SerializeField] protected Animator animator;

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                animator.SetTrigger(Awake);
        }
    }
}