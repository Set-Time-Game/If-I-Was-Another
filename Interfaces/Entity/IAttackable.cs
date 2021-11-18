using static Classes.Utils.Flags;

namespace Interfaces
{
    public interface IAttackable : IDamageable
    {
        public float AttackRadius { get; }
        public float AttackRate { get; }
        public float AttackDistance { get; }
        
        
        public AttackType AttackType { get; }

        public void AttackTypeSwitch(AttackType type);
        public void SwapMelee(AttackType previous);

        public delegate void OnWeaponSwapDelegate(AttackType type);
        public event OnWeaponSwapDelegate onWeaponSwapEvent;
        
        public delegate void OnAttackingDelegate(float damage);
        public event OnAttackingDelegate onAttackingEvent;
    }
}