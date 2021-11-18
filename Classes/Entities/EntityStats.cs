using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Classes.Utils.Flags;

namespace Classes.Entities
{
    public abstract class EntityStats : Entity, IStats
    {
        [Space]
        [SerializeField] protected float currentHealth;
        [SerializeField] protected float maxHealth;
        [SerializeField] protected float currentStamina;
        [SerializeField] protected float maxStamina;
        [SerializeField] protected float currentAlt;
        [SerializeField] protected float maxAlt;
        [SerializeField] protected float currentArmor;
        [SerializeField] protected float currentDamage;
        [SerializeField] protected double currentAgility;
        [SerializeField] protected double currentStrength;
        [SerializeField] protected double currentIntelligence;
        
        protected readonly Dictionary<CoroutineNames, Dictionary<CoroutineTypes, LinkedList<Coroutine>>> Coroutines
            = new Dictionary<CoroutineNames, Dictionary<CoroutineTypes, LinkedList<Coroutine>>>();
        
        protected IEnumerator Regenerate(CoroutineNames coroutineName)
        {
            yield return new WaitForSeconds(3);

            var timer = new WaitForFixedUpdate();

            switch (coroutineName)
            {
                case CoroutineNames.HealthRegeneration:
                    break;
                case CoroutineNames.StaminaRegeneration:
                    const float amount = 0.03f;

                    while (true)
                    {
                        if ((Stamina + amount) / maxStamina >= 100)
                        {
                            Stamina = maxStamina;
                            break;
                        }

                        Stamina += amount;

                        yield return timer;
                    }

                    if (Stamina > maxStamina)
                        Stamina = maxStamina;
                    break;
                case CoroutineNames.AltRegeneration:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(coroutineName), coroutineName, null);
            }
        }
        
        public virtual float Alt
        {
            get => currentAlt;
            protected set => currentAlt = value;
        }

        public virtual float Health
        {
            get => currentHealth;
            set => currentHealth = value;
        }
        
        public float MaxHealth
        {
            get => maxHealth;
            protected set => maxHealth = value;
        }

        public virtual float Stamina
        {
            get => currentStamina;
            protected set => currentStamina = value;
        }
        
        public virtual float Armor => 1 - currentArmor;
        public virtual float Damage => currentDamage;
        public virtual double Agility => currentAgility;
        public virtual double Strength => currentStrength;
        public virtual double Intelligence => currentIntelligence;
    }
}