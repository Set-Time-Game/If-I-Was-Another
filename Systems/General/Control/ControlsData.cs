using System;
using Types.Classes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Systems.General.Control
{
    public class ControlsData : MonoBehaviour
    {
        public ControlStick movement;
        public ControlStick attack;
        [Space] 
        public string move;
        public string roll;
        public string attackMove;
        public string attackAction;
        public string attackSwitch;

        public Guid moveId;
        public Guid rollId;
        public Guid attackMoveId;
        public Guid attackActionId;
        public Guid attackSwitchId;

        public UnityEvent OnRoll;
        public UnityEvent OnAttackSwitch;
        public UnityEvent<Vector2> OnAttackMoveBegin;
        public UnityEvent OnAttackMoveEnd;
        public UnityEvent<Vector2> OnMoveBegin;
        public UnityEvent OnMoveEnd;

        internal float AttackButtonPress;
        
        public void RollCallback(InputAction.CallbackContext ctx)
        {
            var obj = ctx.ReadValueAsObject();
            if (obj is null || ctx.canceled || !ctx.performed || ctx.action.id != rollId)
                return;
            
            OnRoll?.Invoke();
        }

        public void MoveCallback(InputAction.CallbackContext ctx)
        {
            var obj = ctx.ReadValueAsObject();
            if (obj is null || ctx.action.id != moveId)
            {
                OnMoveEnd?.Invoke();
                return;
            }

            var value = ControlStick.SnapInput((Vector2) obj, movement.mode);
            OnMoveBegin?.Invoke(value);
        }

        public void AttackSwitchCallback(InputAction.CallbackContext ctx)
        {
            var obj = ctx.ReadValueAsObject();
            if (obj is null || ctx.canceled || !ctx.performed || ctx.action.id != attackSwitchId)
                return;

            OnAttackSwitch?.Invoke();
        }
        
        public void AttackMoveCallback(InputAction.CallbackContext ctx)
        {
            var obj = ctx.ReadValueAsObject();
            if (ctx.action.id != attackMoveId)
            {
                return;
            }

            if (ctx.canceled)
            {
                OnAttackMoveEnd?.Invoke();
                return;
            }
            
            OnAttackMoveBegin?.Invoke(ControlStick.SnapInput((Vector2) obj, attack.mode));
        }

        public void Awake()
        {
            if (Guid.TryParse(move, out var res))
                moveId = res;
            
            if (Guid.TryParse(roll, out res))
                rollId = res;

            if (Guid.TryParse(attackMove, out res))
                attackMoveId = res;

            if (Guid.TryParse(attackAction, out res))
                attackActionId = res;
            
            if (Guid.TryParse(attackSwitch, out res))
                attackSwitchId = res;
        }
    }
}