using Leopotam.Ecs;
using Types.Structs;

namespace Types.Interfaces
{
    public interface IMapData
    {
        public int MapSize { get; }
        public uint BiomeSize { get; }
        public byte ChunkSize { get; }
        public byte GroundSize { get; }
        
        public BiomeConfig GetBiomeConfig(byte index);
        public EcsComponentRef<Biome> GetBiome(byte index);
        
        public void SetBiome(byte index, EcsComponentRef<Biome> biome);
        public void InitMap();
    }
}