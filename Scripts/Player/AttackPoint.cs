using System;
using System.Collections.Generic;
using System.Diagnostics;
using Classes.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using static Classes.Utils.Flags;
using static Classes.Utils.Utils;

namespace Player
{
    [Serializable]
    public sealed class AttackPoint : MonoBehaviour
    {
        private static Vector2 _coefficient = new Vector2(.1f, .05f);
        [SerializeField] private Character player;
        [SerializeField] private FixedJoystick joystick;
        [SerializeField] private Transform direction;
        [SerializeField] private PlayerCollector collector;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private LayerMask enemyLayer;

        private void Start()
        {
            player.onAttackingEvent += Attack;
            joystick.OnPointerUpEvent += PointerUp;
        }

        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(direction.position, player.AttackRadius);

        private void OnPlayerStateChange(States from, States to)
        {
            if (from == States.Attacking && to == States.Stunned)
                StopAttack();
        }

        private void PointerUp(PointerEventData eventData)
        {
            if (player.State != States.None
                || (collector.Enemies.Count < 1
                    && collector.Resources.Count > 0)) return;

            player.ChangeState(States.Attacking);

            var pointTransform = transform;
            var snapped = joystick.GetSnappedDirection();
            var normalized = snapped.normalized;
            var isHorizontal = Math.Abs(joystick.Direction.x) > Math.Abs(joystick.Direction.y);
            normalized = new Vector2(
                normalized.x != 0 ? normalized.x > 0 ? 1 : -1 : 0,
                normalized.y != 0 ? normalized.y > 0 ? 1 : -1 : 0);

            if (joystick.Direction.y != 0 && joystick.Direction.x != 0)
            {
                pointTransform.localPosition = player.AttackType switch
                {
                    AttackType.Melee => new Vector3
                    (isHorizontal ? normalized.x * .05f: 0,
                        !isHorizontal ? normalized.y * .05f: 0, 0),

                    AttackType.Range => new Vector3
                    (_coefficient.x * snapped.x,
                        _coefficient.y * snapped.y, 0),

                    _ => pointTransform.localPosition
                };
            }

            Rotate(pointTransform, player.Transform);

            //var localPosition = pointTransform.localPosition;

            Flip(player.spriteRenderer,
                normalized,
                player.animator);
        }

        public void Attack(float damage)
        {
            switch (player.AttackType)
            {
                case AttackType.Range:
                {
                    RangeAttack();

                    break;
                }
                case AttackType.Melee:
                {
                    if (collector.Enemies.Count < 1) break;

                    MeleeAttack();

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RangeAttack()
        {
            var pointTransform = transform;
            var bulletInstance = Instantiate(bulletPrefab, pointTransform.position, pointTransform.rotation);

            bulletInstance.player = player;
            bulletInstance.gameObject.SetActive(true);
        }

        private void MeleeAttack()
        {
            var hitEnemies = new Collider2D[collector.Enemies.Count + 1];
            if (Physics2D.OverlapCircleNonAlloc(
                transform.position,
                player.AttackRadius,
                hitEnemies,
                enemyLayer) < 1) return;

            var enemies = new LinkedList<IDamageable>();
            foreach (var enemy in hitEnemies)
                if (enemy != null
                    && enemy.TryGetComponent<IDamageable>(out var damageable)
                    && !enemy.TryGetComponent<Character>(out _)
                    && !enemies.Contains(damageable))
                    enemies.AddLast(damageable);

            if (enemies.Count < 1) return;

            var closest = ClosestFrom(enemies, player.Transform.position);
            closest.TakeDamage(player.Damage);
            enemies.Remove(closest);

            foreach (var enemy in enemies)
                enemy.TakeDamage(player.Damage * .75f);
        }

        public void StopAttack()
        {
            //TODO: stop attack animation and other
        }
    }
}