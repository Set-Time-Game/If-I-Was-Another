using Leopotam.Ecs;
using Types.Classes;
using Types.Interfaces;
using Types.Structs;
using UnityEngine;

namespace Systems
{
    public class MapData : MonoBehaviour, IMapData
    {
        public AstarPath AstarPath;
        
        public uint biomeSize;
        public byte chunkSize;
        public byte groundSize;
        
        public Cluster cluster;
        public BiomeConfig[] biomes;
        public EcsComponentRef<Biome>[] Map;

        public int MapSize { get; private set; }
        public uint BiomeSize => biomeSize;
        public byte ChunkSize => chunkSize;
        public byte GroundSize => groundSize;
        public BiomeConfig GetBiomeConfig(byte index) => biomes[index];
        
        public EcsComponentRef<Biome> GetBiome(byte index) => Map[index];
        public void SetBiome(byte index, EcsComponentRef<Biome> biome) => Map[index] = biome;

        public void InitMap()
        {
            MapSize = biomes.Length;
            Map = new EcsComponentRef<Biome>[MapSize];
        }
    }
}