using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using Classes.Player;
using UI;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using World;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Classes.World
{
    public class World : MonoBehaviour
    {
        public AstarPath pathfinder;
        public Character player;
        public BiomeGenerator[] biomes;

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
            yield return new WaitUntil(() => InventoryManager.Singleton != null);
            var begin = Stopwatch.GetTimestamp();

            foreach (var biome in biomes)
            {
                biome.gameObject.SetActive(true);
                yield return new WaitUntil(() => biome.isGenerated);
            }

            Debug.Log(new TimeSpan(Stopwatch.GetTimestamp() - begin));

            player.GameObject.SetActive(true);

            yield return Resources.UnloadUnusedAssets();

            //pathfinder.Scan();
        }

        public (Texture2D, Texture2D)  DrawNoise()
        {
            return (DrawNoise(Map), DrawNoise(MapDeco));
        }
        
        public Texture2D DrawNoise(float[,] map)
        {
            var width = map.GetLength(0);
            var height = map.GetLength(1);
            var texture = new Texture2D(width, height, TextureFormat.RGB24, false) {filterMode = FilterMode.Point};

            var colorMap = new Color[width * height];

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                var target = map[x, y];
                var res = Color.Lerp(Color.black, Color.white, target);
                foreach (var level 
                    in biomes
                        .Select(generator => generator.objectsLayers)
                        .Select(layers => layers
                            //.Where(level => target <= level.heightTo && target >= level.heightFrom)))
                            .Where(level => target <= level.height)))
                {
                    res = level.Last().color;
                    break;
                }

                colorMap[y * width + x] = res;
            }

            //TODO: change to SetPixels32

            texture.SetPixels(colorMap);
            texture.Apply();

            return texture;
        }
        
        public Texture2D DrawMap(float[,] map)
        {
            var width = map.GetLength(0);
            var height = map.GetLength(1);
            var texture = new Texture2D(width, height) {filterMode = FilterMode.Point};

            var colorMap = new Color[width * height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var target = map[x, y];
                    var res = Color.Lerp(Color.black, Color.white, target);
                    foreach (var level
                        in biomes
                            .Select(generator => generator.objectsLayers)
                            .Select(layers => layers
                                //.Where(level => target <= level.heightTo && target >= level.heightFrom)))
                                .Where(level => target <= level.height)))
                    {
                        res = level.Last().color;
                        break;
                    }

                    colorMap[y * width + x] = res;
                }
            }

            //TODO: change to SetPixels32

            texture.SetPixels(colorMap);
            texture.Apply();

            return texture;
        }
    }
}