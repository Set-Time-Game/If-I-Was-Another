using System;
using System.Collections.Generic;
using System.Linq;
using Classes.Utils;
using UnityEngine;
using World;
using Random = UnityEngine.Random;

namespace Classes.World
{
    public abstract class Prop : MonoBehaviour, IGenerable
    {
        [SerializeField] protected Structs.Variable variety;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] protected BiomeGenerator biome;

        protected virtual void Awake()
        {
            biome = GetComponentInParent<BiomeGenerator>();
        }

        public static void CreateVariable(IEnumerable<Structs.Variable> variablesList, out Structs.Variable variable)
        {
            var value = Random.value;
            var variables = variablesList.ToArray();
            try
            {
                variable = variables.First(x => x.chanceOfSpawn == 0 || x.chanceOfSpawn >= value);
            }
            catch
            {
                variable = variables.First();
            }

        }

        public virtual void Generate(out Structs.Variable variable)
        {
            variable = variety;
        }
        
        public virtual Transform Transform => transform;
        public virtual GameObject GameObject => gameObject;
        public virtual Structs.Variable Variety => variety;

        public virtual SpriteRenderer SpriteRenderer => spriteRenderer;
    }
}