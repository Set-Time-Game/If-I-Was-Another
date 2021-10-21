using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using World;
using Random = UnityEngine.Random;

namespace Classes.World
{
    [Serializable]
    public sealed class ResourceSource : MonoBehaviour, IGenerable
    {
        [SerializeField] private Variable variety;
        [SerializeField] private SpriteRenderer spriteRenderer;
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
            foreach (var i in GetComponentsInParent<Deco>().Select(x => x.gameObject).Where(x => x.transform.localPosition == transform.localPosition))
                Destroy(i);
            
            var biome = GetComponentInParent<BiomeGenerator>();
            var variant = biome.resourceSourceVariablesList[Random.Range(0, biome.resourceSourceVariablesList.Length)];
            var texture = variant.viewsArray[Random.Range(0, variant.viewsArray.Length)];

            spriteRenderer.sprite = texture.defaultTexture;

            variety = variant;
            variety.viewsArray = new[] {texture};
            
            variety.Instance = this;

            variable = variety;
        }

        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
    }
}