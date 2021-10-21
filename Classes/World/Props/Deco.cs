using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using World;
using Random = UnityEngine.Random;

namespace Classes.World
{
    [Serializable]
    public sealed class Deco : MonoBehaviour, IGenerable
    {
        [SerializeField] private Variable variety;
        [SerializeField] private SpriteRenderer spriteRenderer;
        public Variable Variety => variety;
        
        public void Generate(out Variable variable)
        {
            var biome = GetComponentInParent<BiomeGenerator>();
            var variant = biome.decoVariablesList[Random.Range(0, biome.decoVariablesList.Length)];
            var texture = variant.texturesArray[Random.Range(0, variant.texturesArray.Length)];

            spriteRenderer.sprite = texture;

            variety = variant;
            variety.texturesArray = new[] {texture};
            
            variety.Instance = this;

            variable = variety;
        }
        
        public void SetHighlight(bool enable)
        {
        }
        
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
    }
}