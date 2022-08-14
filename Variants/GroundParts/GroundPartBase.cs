using Types.Interfaces;
using Types.Structs;
using UnityEngine;

namespace Variants.GroundParts
{
    [CreateAssetMenu(fileName = "new GroundPart", menuName = "Variants/Ground/GroundPart", order = 0)]
    public class GroundPartBase : ScriptableObject
    {
        public new string name;

        public bool hasCollision;
        public bool hasCollector;

        public PartType type;
        public PartState state;
        
        public Sprite[] TextureArray;
        public GroundPartPairs[] TexturePairs;

        public GroundPart GeneratePart()
        {
            var part = new GroundPart
            {
                Name = name,
                
                Type = type,
                State = state,
                
                HasCollector = hasCollector,
                HasCollision = hasCollision
            };

            if (TextureArray is {Length: > 0})
            {
                part.MainTexture = TextureArray[Random.Range(0, TextureArray.Length)];

                return part;
            }

            if (TexturePairs is not {Length: > 0}) return part;
            
            var pairs = TexturePairs[Random.Range(0, TexturePairs.Length)];
            part.MainTexture = pairs.mainTexture;
            part.HighlightedTexture = pairs.highlightedTexture;
            part.CollectedTexture = pairs.collectedTexture;

            return part;
        }
    }
}
