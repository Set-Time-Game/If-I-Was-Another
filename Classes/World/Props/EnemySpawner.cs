using System;
using System.Linq;
using Classes.Entities;
using Classes.Entities.Enemies;
using UnityEngine;
using UnityEngine.AddressableAssets;
using World;
using Random = UnityEngine.Random;

namespace Classes.World
{
    [Serializable]
    public sealed class EnemySpawner : MonoBehaviour, IGenerable
    {
        [SerializeField] private Variable variety;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Enemy spawnableMob;
        public Variable Variety => variety;
        
        public void SetHighlight(bool enable)
        {
            if (variety.viewsArray.Length <= 0) return;
            
            var texture = variety.viewsArray[0];
            if (texture.outlineTexture || texture.defaultTexture)
                spriteRenderer.sprite = enable && texture.outlineTexture ? texture.outlineTexture : texture.defaultTexture;
        }
        
        public void Generate(out Variable variable)
        {
            var biome = GetComponentInParent<BiomeGenerator>();
            var variant = biome.spawnerVariablesList[Random.Range(0, biome.spawnerVariablesList.Length)];
            var texture = variant.viewsArray[Random.Range(0, variant.viewsArray.Length)];

            spriteRenderer.sprite = texture.defaultTexture;

            variety = variant;
            variety.viewsArray = new[] {texture};
            
            variety.Instance = this;
            
            //TODO: add checking by variant.size
            {
                var ground = biome.MapGroundVariable[Transform.position]
                    .Instance.GameObject
                    .GetComponent<Ground>();

                foreach (var generable in ground.GetComponentsInChildren<IGenerable>().Where(x => x != (IGenerable) this && x != (IGenerable) ground))
                    Destroy(generable.GameObject);
                
            }

            variable = variety;

            var mob = Instantiate(spawnableMob, Transform.position, new Quaternion());
            mob.spawner = this;
        }
        
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
    }
}