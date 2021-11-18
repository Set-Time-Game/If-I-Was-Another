using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using static Classes.Utils.Flags;
using static Classes.Utils.Structs;

namespace Classes.Player
{
    public sealed class Bullet : MonoBehaviour
    {
        public Character player;

        [Space]
        [SerializeField] private Animator animator;
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private EnemyTag[] enemyTags;
        [SerializeField] private string[] destroyTags;
        [SerializeField] private int health;
        [SerializeField] private float speed;
        [SerializeField] private float lifeTime;
        private Coroutine _move;

        private void Awake() => _move = StartCoroutine(Moving());

        private void Start()
        {
            rigidbody.AddForce(
                -transform.right * speed * Time.fixedDeltaTime,
                ForceMode2D.Impulse);
            animator.SetFloat(Health, Utils.Utils.GetPercent(player.Health, player.MaxHealth));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (enemyTags.All(x => !collision.CompareTag(x.name))) return;
            var enemyTag = enemyTags.First(x => collision.CompareTag(x.name));
            health -= enemyTag.hp;

            if (health < 1)
            {
                StopCoroutine(_move);
                Destroy(gameObject);
            }

            if (collision.TryGetComponent<IDamageable>(out var target)) target.TakeDamage(player.Damage);

            if (destroyTags.Contains(collision.tag)) Destroy(collision.gameObject);
        }

        private IEnumerator Moving()
        {
            yield return new WaitForSeconds(lifeTime);

            Destroy(gameObject);
        }
    }
}