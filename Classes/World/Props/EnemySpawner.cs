using System;
using System.Linq;
using Classes.Entities.Enemies;
using UnityEngine;
using World;
using static Classes.Utils.Structs;
using Random = UnityEngine.Random;

namespace Classes.World
{
    [Serializable]
    public class EnemySpawner : SpawnerProp
    {
        public override void Generate(out Variable variable)
        {
            CreateVariable(biome.spawnerVariablesList, out variable);
            var texture = variable.viewsArray[Random.Range(0, variable.viewsArray.Length)];

            spriteRenderer.sprite = texture.defaultTexture;

            variety = variable;
            spawnableMob = texture.spawnableMob;
            variety.viewsArray = new[] {texture};

            variety.Instance = this;

            variable = variety;
            
            base.Generate(out variable);
            Spawn();
        }

        public override GameObject Spawn()
        {
            var mob = base.Spawn();

            if (!mob.TryGetComponent<Enemy>(out var target)) return mob;
            
            var trans = target.Transform;
            var position = trans.position;
                
            target.spawner = this;
            trans.position = position + new Vector3(0, -0.25f, 0);
            
            return mob;
        }
    }
}