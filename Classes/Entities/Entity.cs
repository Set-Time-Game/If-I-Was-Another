using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Classes.Entities
{
    public abstract class Entity : MonoBehaviour, IStats, IDamageable
    {
        public delegate void OnWeaponSwapDelegate(AttackType type);
        public event OnWeaponSwapDelegate OnWeaponSwap;
        
        public delegate void OnStateChangeDelegate(States from, States to);
        public event OnStateChangeDelegate OnStateChange;
        
        public delegate void OnTakeDamageDelegate(float damage);
        public event OnTakeDamageDelegate OnTakeDamage;

        public delegate void OnDieDelegate();
        public event OnDieDelegate OnDie;
        
        [SerializeField] protected float currentHealth;
        [SerializeField] protected float maxHealth;
        [Space]
        [SerializeField] protected float currentStamina;
        [SerializeField] protected float maxStamina;
        [Space]
        [SerializeField] protected float currentAlt;
        [SerializeField] protected float maxAlt;
        [Space]
        [SerializeField] protected float currentArmor;
        [SerializeField] protected float currentDamage;
        [Space]
        [SerializeField] protected double currentAgility;
        [SerializeField] protected double currentStrength;
        [SerializeField] protected double currentIntelligence;

        protected States CurrentState;
        protected AttackType CurrentAttackType;
            
        protected static readonly int vertical = Animator.StringToHash("Vertical");
        protected static readonly int horizontal = Animator.StringToHash("Horizontal");

        public virtual float TakeDamage(float damage)
        {
            damage *= Armor;

            Health -= damage;
            
            OnTakeDamage?.Invoke(damage);
            
            return damage;
        }

        public virtual void ChangeState(States state)
        {
            var fromState = State;
            State = state;
            
            OnStateChange?.Invoke(fromState, state);
        }
        
        public virtual void AttackTypeSwitch(AttackType type)
        {
            if (CurrentAttackType == type)
            {
                SwapMelee(type);
                return;
            }

            CurrentAttackType = type;
            OnWeaponSwap?.Invoke(CurrentAttackType);
        }

        public virtual void SwapMelee(AttackType previous)
        {
            if (CurrentAttackType == AttackType.Melee)
            {
                switch (previous)
                {
                    case AttackType.Melee:
                        break;
                    default:
                        AttackTypeSwitch(previous);
                        break;
                }

                return;
            }

            CurrentAttackType = AttackType.Melee;
            OnWeaponSwap?.Invoke(CurrentAttackType);
        }
        
        public static T ClosestFrom<T>(IEnumerable<T> list, Vector2 target)
            where T : IEntity
        {
            var generables = list as T[] ?? list.ToArray();
            var minValue = Vector2.Distance(target, generables.First().Transform.position);
            var closest = generables.Where(res => Vector2.Distance(target, res.Transform.position) < minValue).ToArray();

            return closest.Length >= 1 ? closest.First() : generables.First();
        }
        
        public static void Flip(ref SpriteRenderer spriteRenderer, Vector2 direction, ref Animator anim,
            bool normalized = true, bool fixd = false)
        {
            var isHorizontal = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
            var flipX = spriteRenderer.flipX;

            flipX = isHorizontal switch
            {
                false when flipX => false,
                true => direction.x < 0,
                _ => false
            };
            spriteRenderer.flipX = flipX;

            anim.SetFloat(horizontal,
                normalized
                    ? isHorizontal
                        ? direction.x > 0
                            ? 1
                            : -1
                        : 0
                    : fixd
                        ? 0
                        : direction.x);
            anim.SetFloat(vertical,
                normalized
                    ? !isHorizontal
                        ? direction.y > 0
                            ? 1
                            : -1
                        : 0
                    : fixd
                        ? 0
                        : direction.y);
        }

        public States State
        {
            get => CurrentState;
            protected set => CurrentState = value;
        }
        
        public AttackType AttackType
        {
            get => CurrentAttackType;
            protected set => CurrentAttackType = value;
        }
        
        public float Armor => currentArmor;
        public virtual float Health
        {
            get => currentHealth;
            protected set => currentHealth = value;
        }

        public virtual float Damage => currentDamage;
        public virtual float Stamina
        {
            get => currentStamina;
            protected set => currentStamina = value;
        }

        public virtual float Alt
        {
            get => currentAlt;
            protected set => currentAlt = value;
        }

        public double Agility => currentAgility;
        public double Strength => currentStrength;
        public double Intelligence => currentIntelligence;

        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
    }
    
    [Flags]
    [Serializable]
    public enum AttackType
    {
        Melee,
        Range,
        Mage
    }

    [Flags]
    [Serializable]
    public enum States
    {
        None,
        Attacking,
        Rolling,
        Stunned,
        Collecting,
        Sleep
    }
}