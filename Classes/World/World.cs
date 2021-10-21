using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using World;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Classes.World
{
    [Serializable]
    public sealed class World : MonoBehaviour
    {
        public enum Layer
        {
            Empty,
            Ground,
            Deco,
            Resource,
            Obstacle,
            Trigger
        }

        public AstarPath pathfinder;

        [Space] public Vector2 zeroPointDeco;
        public Vector2Int sizeDeco;
        public Vector2Int offsetsDeco;
        public int seedDeco;
        public int octavesDeco;
        public float scaleDeco;
        public float persistanceDeco;
        public float lacunarityDeco;

        [Space] public Vector2 zeroPoint;
        public Vector2Int size;
        public Vector2Int offsets;
        public int seed;
        public int octaves;
        public float scale;
        public float persistance;
        public float lacunarity;

        public float[,] Map;
        public float[,] MapDeco;
        public static World Singleton { get; private set; }

        private void Awake()
        {
            if (seed != 0)
                Random.InitState(seed);
            
            NoiseMapGenerator.GenerateNoiseMap(
                size,
                offsets,
                octaves,
                scale,
                persistance,
                lacunarity,
                out Map);
            
            NoiseMapGenerator.GenerateNoiseMap(
                sizeDeco,
                offsetsDeco, 
                octavesDeco,
                scaleDeco,
                persistanceDeco,
                lacunarityDeco,
                out MapDeco);

            Singleton = this;
        }

        private IEnumerator Start()
        {
            var begin = Stopwatch.GetTimestamp();
            
            foreach (var biome in GetComponentsInChildren<BiomeGenerator>(true))
            {
                biome.gameObject.SetActive(true);
                yield return new WaitUntil(() => biome.isGenerated);
            }
            
            Debug.Log(new TimeSpan(Stopwatch.GetTimestamp() - begin));
            
            pathfinder.Scan();
        }
    }
}