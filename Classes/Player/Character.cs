using System;
using System.Collections;
using Classes.Entities;
using Classes.UI;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Classes.Player
{
    public sealed class Character : Entity
    {
        [Space]
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public Rigidbody2D rigidbody;
        [Space] 
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider staminaBar;
        [SerializeField] private Slider altBar;
        [SerializeField] private Slider xpBar;

        private static readonly int AttackMode = Animator.StringToHash("AttackMode");
        private static readonly int Roll = Animator.StringToHash("Roll");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Collect = Animator.StringToHash("Collect");

        private IEnumerator Start()
        {
            yield return null;
            
            animator.SetFloat(vertical, -1);
            animator.SetFloat(horizontal, 0);
        }

        public override void ChangeState(States state)
        {
            base.ChangeState(state);
            var trigger = state switch
            {
                States.Attacking => Attack,
                States.Rolling => Roll,
                States.Collecting => Collect,
                States.None => -1,
                States.Stunned => -1,
                States.Sleep => -1,
                _ => -1
            };
            
            if (trigger != -1)
                animator.SetTrigger(trigger);
        }

        public void AttackTypeSwitch(WeaponChanger source)
        {
            AttackTypeSwitch(source.buttonType);
        }
        
        public override void AttackTypeSwitch(AttackType type)
        {
            if (CurrentAttackType != type)
                animator.SetFloat(AttackMode, (float) type);
            
            base.AttackTypeSwitch(type);
        }

        public override void SwapMelee(AttackType previous)
        {
            if (CurrentAttackType != AttackType.Melee)
                animator.SetFloat(AttackMode, (float) AttackType.Melee);
            
            base.SwapMelee(previous);
        }

        public override float Health
        {
            get => currentHealth;
            protected set
            {
                currentHealth = value;
                healthBar.SetValueWithoutNotify(currentHealth / maxHealth);
            }
        }
        
        public override float Stamina
        {
            get => currentStamina;
            protected set
            {
                currentStamina = value;
                staminaBar.SetValueWithoutNotify(currentStamina / maxStamina);
            }
        }
        
        public override float Alt
        {
            get => currentAlt;
            protected set
            {
                currentAlt = value;
                altBar.SetValueWithoutNotify(currentAlt / maxAlt);
            }
        }
    }
}