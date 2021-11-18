using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes.Entities;
using Classes.Utils;
using UI;
using UnityEngine;
using UnityEngine.UI;
using static Classes.Utils.Flags;
using static Classes.Utils.Utils;

namespace Classes.Player
{
    public sealed class Character : EntityAttackable
    {
        public delegate void OnCollectingEventDelegate();
        public event OnCollectingEventDelegate onCollectingEvent;
        
        [Space] public Animator animator;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider staminaBar;
        [SerializeField] private Slider altBar;
        [SerializeField] private Slider xpBar;

        private void Collecting() => onCollectingEvent?.Invoke();
        private void RollingState() => Stamina -= 20;
        public void AttackTypeSwitch(WeaponChanger source) => AttackTypeSwitch(source.buttonType);
        
        private void Awake()
        {
            onStateChangeEvent +=
                (from, to) =>
                {
                    int trigger;

                    switch (to)
                    {
                        case States.Attacking:
                            trigger = Attack;
                            break;
                        case States.Rolling:
                            RollingState();
                            trigger = Roll;
                            break;
                        case States.Collecting:
                            trigger = Collect;
                            break;
                        default:
                            trigger = -1;
                            break;
                    }

                    if (trigger != -1)
                        animator.SetTrigger(trigger);
                };

            onStateChangeEvent +=
                (from, to) =>
                {
                    if (from != States.Rolling) return;
                    
                    if (!Coroutines.ContainsKey(CoroutineNames.StaminaRegeneration))
                        Coroutines[CoroutineNames.StaminaRegeneration] =
                            new Dictionary<CoroutineTypes, LinkedList<Coroutine>>();

                    if (!Coroutines[CoroutineNames.StaminaRegeneration].ContainsKey(CoroutineTypes.Passive))
                        Coroutines[CoroutineNames.StaminaRegeneration][CoroutineTypes.Passive] =
                            new LinkedList<Coroutine>();

                    var address = Coroutines[CoroutineNames.StaminaRegeneration][CoroutineTypes.Passive];

                    if (Coroutines[CoroutineNames.StaminaRegeneration][CoroutineTypes.Passive].Count > 0)
                    {
                        var coroutine = address.First();

                        StopCoroutine(coroutine);
                        address.Remove(coroutine);
                    }

                    address.AddFirst(StartCoroutine(Regenerate(CoroutineNames.StaminaRegeneration)));
                };
        }

        private IEnumerator Start()
        {
            yield return null;

            animator.SetFloat(Vertical, -1);
            animator.SetFloat(Horizontal, 0);
        }

        public override void AttackTypeSwitch(AttackType type)
        {
            base.AttackTypeSwitch(type);
            
            animator.SetFloat(AttackMode, (int) AttackType);
        }

        public override void SwapMelee(AttackType previous)
        {
            base.SwapMelee(previous);
            if (currentAttackType != AttackType.Melee)
                animator.SetFloat(AttackMode, (float) AttackType.Melee);
        }
        
        public override float Health
        {
            get => currentHealth;
            set
            {
                currentHealth = value;
                healthBar.SetValueWithoutNotify(Health / MaxHealth);
                animator.SetFloat(Flags.Health, GetPercent(Health, MaxHealth));
            }
        }
        
        public override float Stamina
        {
            get => currentStamina;
            protected set
            {
                currentStamina = value;
                staminaBar.SetValueWithoutNotify(Stamina / maxStamina);
            }
        }

        public override float Alt
        {
            get => currentAlt;
            protected set
            {
                currentAlt = value;
                altBar.SetValueWithoutNotify(Alt / maxAlt);
            }
        }
    }
}