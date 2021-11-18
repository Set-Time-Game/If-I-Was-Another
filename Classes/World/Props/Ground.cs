using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using World;
using static Classes.Utils.Structs;
using Random = UnityEngine.Random;

namespace Classes.World
{
    
    [Serializable]
    public sealed class Ground : Prop
    {
        private const float Distance = .25f;
        [SerializeField] private GameObject partGroup;
        [SerializeField] private GameObject[] parts;
        [SerializeField] private GameObject[] other;

        private Dictionary<Vector2, int> _directions = new Dictionary<Vector2, int>()
        {
            {new Vector2(-Distance, Distance), 0},
            {new Vector2(Distance, Distance), 1},
            {new Vector2(Distance, -Distance), 2},
            {new Vector2(-Distance, -Distance), 3},
        };

        public void DisablePlacing()
        {
            Destroy(partGroup);
            partGroup = null;
            parts = null;
            other = null;
            
            variety.canPlacing = false;
        }

        public override void Generate(out Variable variable)
        {
            CreateVariable(biome.groundVariablesList, out variable);
            var texture = variable.texturesArray[Random.Range(0, variable.texturesArray.Length)];

            if (!variable.canPlacing)
            {
                DisablePlacing();
            } else {
                for (var i = 0; i < 4; i++)
                {
                    if (Random.Range(0.0f, 1.0f) > (.20f + (.15f * i))) continue;
                    
                    Destroy(parts[i]);
                    parts[i] = null;
                }
            }

            spriteRenderer.sprite = texture;

            variety = variable;
            variety.texturesArray = new[] {texture};

            variety.Instance = this;

            variable = variety;
            base.Generate(out variable);
        }
        
        public void DisablePlacing(Vector2 volume, Vector2 size, Vector2Int intVolume)
        {
            var indexses = new LinkedList<int>();
            
            if (size - new Vector2(Mathf.Abs(volume.x), Mathf.Abs(volume.y)) == Vector2.zero && intVolume != size)
            {
                if (other != null && other.Length > 0)
                {
                    foreach (var i in other)
                        Destroy(i);

                    other = null;
                }

                if (Mathf.Abs(volume.x).Equals(Mathf.Abs(volume.y)))
                {
                    indexses.AddLast(volume.x > 0 
                        ? (volume.y < 0 
                            ? _directions[new Vector2(-Distance, Distance)] 
                            : _directions[new Vector2(-Distance, -Distance)]) 
                            
                        : (volume.y < 0 
                            ? _directions[new Vector2(Distance, Distance)]
                            : _directions[new Vector2(Distance, -Distance)]));
                    
                } else
                {
                    // 50%
                    if (volume.x > 0 && volume.y == 0)
                    {
                        indexses.AddLast(_directions[new Vector2(-Distance, Distance)]);
                        indexses.AddLast(_directions[new Vector2(-Distance, -Distance)]);
                    } else if (volume.x == 0 && volume.y > 0)
                    {
                        indexses.AddLast(_directions[new Vector2(-Distance, Distance)]);
                        indexses.AddLast(_directions[new Vector2(Distance, Distance)]);
                    } else if (volume.x < 0 && volume.y == 0)
                    {
                        indexses.AddLast(_directions[new Vector2(Distance, Distance)]);
                        indexses.AddLast(_directions[new Vector2(Distance, -Distance)]);
                    } else if (volume.x == 0 && volume.y < 0)
                    {
                        indexses.AddLast(_directions[new Vector2(-Distance, -Distance)]);
                        indexses.AddLast(_directions[new Vector2(Distance, -Distance)]);
                    }
                }
            } else
            {
                DisablePlacing();
            }

            foreach (var i in indexses.Where(i => parts != null && parts.Length >= _directions.Last().Value))
            {
                if (parts[i] == null) continue;
                
                Destroy(parts[i]);
                parts[i] = null;
            }
            
            biome.MapGroundVariable[Transform.position] = variety;
        }
    }
}