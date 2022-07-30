using Leopotam.Ecs;

namespace Types.Structs
{
    public struct Chunk
    {
        public uint Id;
        public byte BiomeId;
        
        public EcsComponentRef<Ground>[] Grounds;
    }
}