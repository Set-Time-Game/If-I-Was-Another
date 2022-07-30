using System;
using System.Collections;
using System.Threading;
using Systems.General.Control;
using UnityEngine;
using UnityEngine.Events;

namespace Types.Classes
{
    [Serializable]
    public class PlayerController : MonoBehaviour
    {
        public Player player;
        public ControlsData controlsData;

        public UnityEvent<Vector2> onPlayerAttack;
        public UnityEvent<Vector2> onPlayerMoveBegin;
        public UnityEvent onPlayerMoveEnd;
        public UnityEvent<Vector2> onPlayerAttackMoveBegin;
        public UnityEvent onPlayerRoll;
        public UnityEvent<AttackMode, AttackMode> onPlayerAttackSwitch;

        public AnimationCurve rollButtonAmplifier;

        private Coroutine m_moveCoroutine;

        private CancellationTokenSource m_moveToken = new();
        private CancellationTokenSource m_rollToken = new();
        private static CancellationTokenSource m_attackToken = new();

        private readonly WaitForEndOfFrame m_moveTimer = new();
        private readonly WaitForFixedUpdate m_rollTimer = new();

        private Vector2 m_attackInput;
        private Vector2 m_moveInput = new(0, -1);

        public void SwitchAttackMode()
        {
            var mode = player.AttackMode == AttackMode.Melee ? AttackMode.Range : AttackMode.Melee;
            onPlayerAttackSwitch?.Invoke(player.AttackMode, mode);
            player.AttackMode = mode;
        }

        public void AttackMove(Vector2 attackInput)
        {
            m_attackInput = attackInput;
            onPlayerAttackMoveBegin?.Invoke(m_attackInput);
        }

        public void TryAttack()
        {
            if (player.State != PlayerStates.None)
                return;

            onPlayerAttack?.Invoke(m_attackInput);

            TryStopMove();
            m_attackToken ??= new CancellationTokenSource();
        }

        public void TryRoll()
        {
            if (player.State != PlayerStates.None)
                return;

            player.State = PlayerStates.Roll;

            TryStopMove();
            onPlayerRoll.Invoke();
            m_rollToken = new CancellationTokenSource();
            player.StartCoroutine(DoRoll(m_moveInput));
        }
        
        private IEnumerator DoRoll(Vector2 direction)
        {
            yield return null;

            var time = 0f;
            var amplifier = rollButtonAmplifier.Evaluate(time);
            while (!m_rollToken.Token.IsCancellationRequested && time < 1f)
            {
                player.rigidbody2D.MovePosition(((Vector2) player.transform.position) +
                                                (new Vector2(amplifier, amplifier) * direction * .1f));

                time += Time.fixedDeltaTime;
                if (time > 1)
                    time = 1;
                amplifier = rollButtonAmplifier.Evaluate(time);
                yield return m_rollTimer;
            }

            player.State = PlayerStates.None;
        }

        public void TryMove(Vector2 moveInput)
        {
            if (player.State != PlayerStates.None)
                return;

            m_moveInput = moveInput;
            onPlayerMoveBegin?.Invoke(m_moveInput);
            
            if (m_moveCoroutine is not null) return;
            
            m_moveToken = new CancellationTokenSource();
            m_moveCoroutine = player.StartCoroutine(DoMove());
        }

        public void TryStopMove()
        {
            m_moveToken?.Cancel();
            onPlayerMoveEnd?.Invoke();

            if (m_moveCoroutine == null) return;

            player.StopCoroutine(m_moveCoroutine);
            m_moveCoroutine = null;
        }

        private IEnumerator DoMove()
        {
            yield return null;

            while (!m_moveToken.Token.IsCancellationRequested)
            {
                player.rigidbody2D.MovePosition((Vector2) player.transform.position + (m_moveInput * .05f));

                yield return m_moveTimer;
            }
        }
    }
}