using System.Collections;
using UnityEngine;

namespace Classes.World
{
    public abstract class SpawnerProp : BigProp
    {
        [SerializeField] protected GameObject spawnableMob;
        
        private Coroutine _respawn;

        public virtual GameObject Spawn()
        {
            return Instantiate(spawnableMob, Transform);
        }
        
        public virtual void Respawn(float time = 5)
        {
            _respawn = StartCoroutine(Respawn((int)time));
        }

        private IEnumerator Respawn(int time = 5)
        {
            yield return new WaitForSeconds(time);
            Spawn();
        }
    }
}