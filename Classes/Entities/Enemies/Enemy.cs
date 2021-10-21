using System;
using Classes.World;
using UnityEngine;

namespace Classes.Entities.Enemies
{
    public class Enemy : Entity
    {
        public EnemySpawner spawner;

        private void Awake()
        {
            OnDie += () => { Instantiate(this, spawner != null ? spawner.Transform.position : Vector3.zero, new Quaternion()); Destroy(this);};
        }
    }
}