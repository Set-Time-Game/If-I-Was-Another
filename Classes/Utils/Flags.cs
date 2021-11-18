using System;
using UnityEngine;

namespace Classes.Utils
{
    public static class Flags
    {
        [Serializable]
        public enum AttackType
        {
            Melee = 0,
            Range = 1,
            Mage = 2
        }

        [Flags]
        [Serializable]
        public enum MobTypes
        {
            Neutral,
            Enemy,
            Animal,
            Humanoid
        }

        [Serializable]
        public enum CoroutineNames
        {
            HealthRegeneration,
            StaminaRegeneration,
            AltRegeneration,
        }

        [Serializable]
        public enum CoroutineTypes
        {
            Passive,
            Active,
            Buff,
        }

        [Flags]
        [Serializable]
        public enum Layer
        {
            Empty,
            Ground,
            Deco,
            Resource,
            Obstacle,
            Trigger
        }

        //[Flags]
        [Serializable]
        public enum ObjectType
        {
            Ground,
            Obstacle,
            ResourceSource,
            Deco,
            Spawner
        }

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

        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int Awake = Animator.StringToHash("Awake");
        public static readonly int AttackMode = Animator.StringToHash("AttackMode");
        public static readonly int Collect = Animator.StringToHash("Collect");
        public static readonly int Damaged = Animator.StringToHash("Damaged");
        public static readonly int Death = Animator.StringToHash("Death");
        public static readonly int FlyUp = Animator.StringToHash("FlyUp");
        public static readonly int Health = Animator.StringToHash("Health");
        public static readonly int Horizontal = Animator.StringToHash("Horizontal");
        public static readonly int Roll = Animator.StringToHash("Roll");
        public static readonly int Sleep = Animator.StringToHash("Sleep");
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int Vertical = Animator.StringToHash("Vertical");
    }
}