using UnityEngine;

namespace Types.Structs
{
    public struct GroundPart
    {
        public bool HasCollision;
        public bool HasCollector;
        
        public PartType Type;
        public PartState State;
        
        public Sprite MainTexture;
        public Sprite HighlightedTexture;
        public Sprite PickedTexture;

        public GroundPart(GroundPartConfig config)
        {
            State = config.state;
            Type = config.type;
            
            HasCollision = config.hasCollision;
            HasCollector = config.hasCollector;
            
            HighlightedTexture = null;
            PickedTexture = null;
            MainTexture = null;

            if (config.textureVariants != null && config.textureVariants.Length > 0)
            {
                MainTexture = config.textureVariants[Random.Range(0, config.textureVariants.Length)];
            }
            else if (config.pairsVariantas != null && config.pairsVariantas.Length > 0)
            {
                var pairs = config.pairsVariantas[Random.Range(0, config.pairsVariantas.Length)];
                MainTexture = pairs.mainTexture;
                HighlightedTexture = pairs.highlightedTexture;
                PickedTexture = pairs.pickedTexture;
            }
        }
    }
}