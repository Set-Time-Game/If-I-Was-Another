using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Saves;
using UnityEngine;
using UnityEngine.AddressableAssets;
using World;
using Random = UnityEngine.Random;

namespace Classes.World
{
    [Serializable]
    public sealed class Ground : MonoBehaviour, IGenerable
    {
        [SerializeField] private Variable variety;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GameObject partGroup;
        [SerializeField] private GameObject[] parts;
        public Variable Variety => variety;

        private const float Distance = .25f;
        private Dictionary<Vector2, int> _directions = new Dictionary<Vector2, int>()
        {
            {new Vector2(-Distance, Distance), 0},
            {new Vector2(Distance, Distance), 1},
            {new Vector2(Distance, -Distance), 2},
            {new Vector2(-Distance, -Distance), 3},
        };

        public void Generate(out Variable variable)
        {
            var biome = GetComponentInParent<BiomeGenerator>();
            var variant = biome.groundVariablesList[Random.Range(0, biome.groundVariablesList.Length)];
            var texture = variant.texturesArray[Random.Range(0, variant.texturesArray.Length)];
            
            if (!variant.canPlacing)
                    Destroy(partGroup);
            
            for (var i = 0; i < 4; i++)
                if (Random.Range(0.0f, 1.0f) <= (.3f + (.05f * i)))
                    Destroy(parts[i].gameObject);

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