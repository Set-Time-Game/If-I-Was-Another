using UnityEngine;

namespace Types.Structs
{
    public struct Ground
    {
        public byte Id;
        public uint ChunkId;
        public byte BiomeId;

        public Sprite MainTexture;
        public GroundPart[] Parts;
    }
}