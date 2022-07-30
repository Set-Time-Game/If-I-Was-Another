using System;

namespace Types.Structs
{
    [Serializable]
    public struct BiomeConfig
    {
        public GroundConfig[] groundConfigs;
        public GroundPartConfig[] groundPartConfigs;
    }
}