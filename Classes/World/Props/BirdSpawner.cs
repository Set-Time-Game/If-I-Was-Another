using System;
using System.Collections;
using Classes.Entities.Animals;
using UnityEngine;
using UnityEngine.GameFoundation;
using Random = UnityEngine.Random;

namespace Classes.World
{
    public class BirdSpawner : EnemySpawner
    {
        private IEnumerator Start()
        {
            if (Camera.main is null) yield break;

            yield return new WaitForEndOfFrame();
            
            Spawn();
        }

        public override GameObject Spawn()
        {
            var screen = Camera.main.ScreenToWorldPoint(
                new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0));
            screen.z = 0;
            
            transform.position = screen;

            var mob = base.Spawn();
            if (!mob.TryGetComponent<Bird>(out var bird)) return mob;

            bird.spawner = this;
            
            return bird.GameObject;
        }
    }
}