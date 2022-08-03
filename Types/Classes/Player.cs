using System;
using Systems.General.Control;
using UnityEngine;
using UnityEngine.Events;

namespace Types.Classes
{
    [Serializable]
    public class Player : MonoBehaviour
    {
        public UnityEvent AttackTrigger;
        
        public PlayerStates State;
        public AttackMode AttackMode;

        public Animator animator;
        public Transform Transform;
        public SpriteRenderer spriteRenderer;
        public CapsuleCollider2D capsuleCollider;

        public new Transform transform;
        public new Rigidbody2D rigidbody2D;

        public PlayerController Controller;

        public static readonly int PositionX = Animator.StringToHash("X_Position");
        public static readonly int PositionY = Animator.StringToHash("Y_Position");
        public static readonly int Roll = Animator.StringToHash("Roll");
        public static readonly int Attack = Animator.StringToHash("Attack");
        public readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int AttackModeHash = Animator.StringToHash("AttackMode");

        public void AnimationAttack()
        {
            AttackTrigger?.Invoke();
        }
        
        public void SetAttack(Vector2 attackInput)
        {
            SetTrigger(PlayerStates.Attack);
            SetAnimatorPosition(ControlStick.SnapInput(attackInput, AxisOptions.Fixed));
        }
        
        public void SetRoll() 
            => SetTrigger(PlayerStates.Roll);

        public void SetMove(Vector2 moveInput)
        {
            SetAnimatorPosition(moveInput);
            SetSpeed(SpeedMode.Move);
        }

        public void StopMove() => SetSpeed(SpeedMode.Idle);

        public void SetAttack(AttackMode _, AttackMode mode) 
            => animator.SetFloat(AttackModeHash, (byte) mode);


        private void SetAnimatorPosition(Vector2 position)
        {
            animator.SetFloat(PositionX, position.x);
            animator.SetFloat(PositionY, position.y);
            FlipSprite(position);
        }

        private void SetSpeed(SpeedMode amount) 
            => animator.SetFloat(Speed, (byte) amount);

        public void SetTrigger(PlayerStates state)
        {
            int trigger;
            switch (state)
            {
                case PlayerStates.None:
                    trigger = -1;
                    State = PlayerStates.None;
                    break;
                case PlayerStates.Roll:
                    trigger = Roll;
                    State = PlayerStates.Roll;
                    break;
                case PlayerStates.Attack:
                    trigger = Attack;
                    State = PlayerStates.Attack;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            if (trigger == -1)
                return;
            
            animator.SetTrigger(trigger);
            FlipSprite(GetAnimatorPosition());
        }

        
        private void FlipSprite(Vector2 position) 
            => FlipSprite(position.x < -0.7f);

        private void FlipSprite(bool state) 
            => spriteRenderer.flipX = state;

        private Vector2 GetAnimatorPosition() 
            => new Vector2(animator.GetFloat(PositionX), animator.GetFloat(PositionY));
    }

    [Serializable]
    public enum PlayerStates : byte
    {
        None,
        Roll,
        Attack
    }

    [Serializable]
    public enum AttackMode : byte
    {
        Melee,
        Range
    }

    [Serializable]
    public enum SpeedMode : byte
    {
        Idle,
        Move
    }
}