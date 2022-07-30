using Leopotam.Ecs;

namespace Types.Structs
{
    public struct Biome
    {
        public byte Id;

        public EcsComponentRef<Chunk>[] Chunks;

        public void SetChunks(EcsComponentRef<Chunk>[] chunks)
        {
            Chunks = chunks;
        }

        public void InitChunks(uint size)
        {
            SetChunks(new EcsComponentRef<Chunk>[size]);
        }
    }
}