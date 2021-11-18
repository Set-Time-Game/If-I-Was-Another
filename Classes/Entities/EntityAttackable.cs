using Interfaces;
using UnityEngine;
using static Classes.Utils.Flags;

namespace Classes.Entities
{
    public abstract class EntityAttackable : EntityDamageable, IAttackable
    {
        public virtual event IAttackable.OnAttackingDelegate onAttackingEvent;
        public virtual event IAttackable.OnWeaponSwapDelegate onWeaponSwapEvent;
        
        [Space]
        [SerializeField] protected float currentAttackRadius;
        [SerializeField] protected float currentAttackRate;
        [SerializeField] protected float currentAttackDistance;
        [SerializeField] protected AttackType currentAttackType;
        
        public virtual float AttackRadius => currentAttackRadius;
        public virtual float AttackRate => currentAttackRate;
        public virtual float AttackDistance => currentAttackDistance;
        
        public virtual AttackType AttackType => currentAttackType;
        
        protected virtual void Attacking() => onAttackingEvent?.Invoke(Damage);
        
        public virtual void AttackTypeSwitch(AttackType type)
        {
            if (currentAttackType == type)
            {
                SwapMelee(type);
                return;
            }

            currentAttackType = type;
            onWeaponSwapEvent?.Invoke(currentAttackType);
        }

        public virtual void SwapMelee(AttackType previous)
        {
            if (currentAttackType == AttackType.Melee)
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

            currentAttackType = AttackType.Melee;
            onWeaponSwapEvent?.Invoke(currentAttackType);
        }
    }
}