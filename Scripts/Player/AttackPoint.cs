using System;
using System.Collections;
using System.Diagnostics;
using Classes.Entities;
using Classes.Player;
using Classes.World;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player
{
    [Serializable]
    public sealed class AttackPoint : MonoBehaviour
    {
        [SerializeField] private Character player;
        [SerializeField] private FixedJoystick joystick;
        [SerializeField] private Transform direction;

        private Coroutine _attack;
        
        private static readonly Vector2 Coefficient = new Vector2(.1f, .05f);
        private static readonly Vector2 Multiplier = new Vector2(1 / Coefficient.x, 1 / Coefficient.y);

        private void Start() => joystick.OnPointerUpEvent += PointerUp;

        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(direction.position, .25f);

        private void OnPlayerStateChange(States from, States to)
        {
            if (from == States.Attacking && to == States.Stunned && _attack != null)
                StopAttack();
        }
        
        private void PointerUp(PointerEventData eventData)
        {
            if (player.State != States.None) return;
            
            player.ChangeState(States.Attacking);

            var verticalPosition = joystick.Direction.y;
            var horizontalPosition = joystick.Direction.x;
            var yNormalized = verticalPosition > 0 ? 1 : -1;
            var xNormalized = horizontalPosition > 0 ? 1 : -1;
            var snapped = joystick.GetSnappedDirection();
            var isHorizontal = Math.Abs(horizontalPosition) > Math.Abs(verticalPosition);

            if (verticalPosition != 0 && horizontalPosition != 0)
                transform.localPosition = player.AttackType switch
                {
                    AttackType.Melee => new Vector3
                    (isHorizontal ? Coefficient.x * xNormalized : 0,
                        !isHorizontal ? Coefficient.y * yNormalized : 0, 0),

                    AttackType.Range => new Vector3
                    (Coefficient.x * snapped.x,
                        Coefficient.y * snapped.y, 0),

                    _ => transform.localPosition
                };

            Rotate(player.Transform, transform);

            var localPosition = transform.localPosition;
            
            Entity.Flip(ref player.spriteRenderer, 
                new Vector2(localPosition.x * Multiplier.x, localPosition.y * Multiplier.y),
                ref player.animator,
                false);
            
            _attack = StartCoroutine(AttackRoutine());
        }

        private IEnumerator AttackRoutine()
        {
            yield return null;
        }

        private void StopAttack()
        {
            
        }

        private static void Rotate(Transform pointer, Transform target)
        {
            var diff = pointer.localPosition - target.position;
            diff.Normalize();

            target.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
        }
    }
}