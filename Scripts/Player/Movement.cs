using System;
using System.Collections;
using Classes.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Classes.Utils.Flags;
using static Classes.Utils.Utils;

namespace Player
{
    public sealed class Movement : MonoBehaviour
    {
        [SerializeField] private Character player;
        [SerializeField] private FixedJoystick joystick;
        [SerializeField] private AnimationCurve rollingMultiplier;
        [SerializeField] private Button rollButton;
        private readonly TimeSpan _rollCd = TimeSpan.FromSeconds(3);
        private Coroutine _moving;

        private Coroutine _rolling;

        private DateTime _rollUsage;

        private void Start()
        {
            player.onStateChangeEvent += (from, to) =>
            {
                if (from != States.Rolling) return;
                StartCoroutine(RollingStop());
            };
            joystick.OnPointerDownEvent += PointerDown;
            joystick.OnPointerUpEvent += PointerUp;
        }

        public void Roll()
        {
            if (player.State != States.None || player.Stamina < 20) return;

            _rolling = StartCoroutine(
                Roll(
                    new Vector2(
                        player.animator.GetFloat(Horizontal),
                        player.animator.GetFloat(Vertical))));
        }

        private IEnumerator Roll(Vector2 direction)
        {
            rollButton.interactable = false;
            _rollUsage = DateTime.UtcNow;

            player.ChangeState(States.Rolling);

            var timer = new WaitForFixedUpdate();

            while (true)
            {
                const float additionMultiplier = .12f; //1.3f;

                var pos = transform.position;
                var multiplier =
                    rollingMultiplier.Evaluate(player.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

                var x = multiplier * ((direction.x != 0 ? direction.x : 0));
                var y = multiplier * ((direction.y != 0 ? direction.y : 0));

                player.rigidbody.MovePosition(new Vector2(pos.x + x * additionMultiplier,
                    pos.y + y * additionMultiplier));

                yield return timer;
            }
        }

        private IEnumerator RollingStop()
        {
            if (_rolling != null)
            {
                StopCoroutine(_rolling);
                _rolling = null;
            }

            //player.animator.SetTrigger(StopRolling1);
            player.ChangeState(States.None);

            /*while (((DateTime.UtcNow - _rollUsage > _rollCd) && (player.Stamina < 20)))
            {
                yield return new WaitForEndOfFrame();
            }*/
            
            yield return new WaitUntil(() => ((DateTime.UtcNow - _rollUsage > _rollCd) && (player.Stamina >= 20)));

            rollButton.interactable = true;
        }

        private void PointerDown(PointerEventData eventData)
        {
            player.animator.SetFloat(Speed, 1);
            StopMoving();
            _moving = StartCoroutine(CharacterMove());
        }

        private void PointerUp(PointerEventData eventData)
        {
            player.animator.SetFloat(Speed, 0);
            StopMoving();
        }

        private IEnumerator CharacterMove()
        {
            var waiter = new WaitForFixedUpdate();

            while (true)
            {
                if (player.State == States.None
                    && (joystick.Direction.y != 0 || joystick.Direction.x != 0))
                {
                    var direction = joystick.Direction;

                    Flip(player.spriteRenderer, direction, player.animator, false, false);

                    direction = joystick.GetSnappedDirection();

                    //player.Transform.Translate(direction * Time.fixedDeltaTime);
                    player.rigidbody.MovePosition((direction * Time.fixedDeltaTime) +
                                                  (Vector2) player.Transform.position);
                }

                yield return waiter;
            }
        }

        private void StopMoving()
        {
            if (_moving == null) return;

            StopCoroutine(_moving);
            _moving = null;
        }
    }
}