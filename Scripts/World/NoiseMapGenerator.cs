using UnityEngine;

namespace World
{
    //TODO: change to some IJob with burs improve
    public static class NoiseMapGenerator
    {
        public static void GenerateNoiseMap(Vector2Int size, Vector2Int offsets, int octaves, float scale,
            float persistance, float lacunarity, out float[,] map)
        {
            var width = size.x;
            var height = size.y;
            map = new float[width, height];

            if (scale <= 0) scale = 0.0001f;
            var octavesOffset = new Vector2[octaves];
            for (var i = 0; i < octaves; i++)
            {
                var xOffset = (float) (Random.Range(-100000, 100000) + offsets.x);
                var yOffset = (float) (Random.Range(-100000, 100000) + offsets.y);
                octavesOffset[i] = new Vector2(xOffset / size.x, yOffset / size.y);
            }

            var max = float.MinValue;
            var min = float.MaxValue;

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var amplitude = 1f;
                    var frequency = 1f;
                    var noiseHeight = 0f;

                    for (var i = 0; i < octaves; i++)
                    {
                        noiseHeight += (
                            Mathf.PerlinNoise(
                                x / scale * frequency + octavesOffset[i].x,
                                y / scale * frequency + octavesOffset[i].y) //* 2 - 1
                            ) * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > max)
                        max = noiseHeight;
                    else if (noiseHeight < min) min = noiseHeight;

                    map[x, y] = noiseHeight;
                }
            }


            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                map[x, y] = Mathf.InverseLerp(min, max, map[x, y]);
        }
    }
}