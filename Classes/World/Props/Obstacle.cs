using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using World;
using Random = UnityEngine.Random;

namespace Classes.World
{
    [Serializable]
    public sealed class Obstacle : MonoBehaviour, IGenerable
    {
        [SerializeField] private Variable variety;
        [SerializeField] private SpriteRenderer spriteRenderer;
        public Variable Variety => variety;
        
        private const float Distance = .25f;
        
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
            var variant = biome.obstacleVariablesList[Random.Range(0, biome.obstacleVariablesList.Length)];
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
        }
        
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
    }
}