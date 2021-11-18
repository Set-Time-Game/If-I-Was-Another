using Interfaces;
using UnityEngine;
using static Classes.Utils.Flags;

namespace Classes.Entities
{
    public abstract class EntityDamageable : EntityStats, IDamageable
    {
        public virtual event IDamageable.OnDieDelegate onDieEvent;
        public virtual event IDamageable.OnTakeDamageDelegate onTakeDamageEvent;
        public virtual event IState.OnStateChangeDelegate onStateChangeEvent;
        
        [Space]
        [SerializeField] protected States currentState;
        
        public virtual States State
        {
            get => currentState;
            protected set => currentState = value;
        }
        
        public virtual float TakeDamage(float damage)
        {
            damage *= Armor;

            Health -= damage;

            onTakeDamageEvent?.Invoke(damage);

            if (Health < 1)
                onDieEvent?.Invoke();

            return damage;
        }
        
        public virtual void ChangeState(States state)
        {
            var fromState = State;
            State = state;

            onStateChangeEvent?.Invoke(fromState, state);
        }
    }
}