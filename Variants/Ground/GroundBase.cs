using UnityEngine;

namespace Variants.Ground
{
    [CreateAssetMenu(fileName = "new Ground", menuName = "Variants/Ground/Ground", order = 0)]
    public class GroundBase : ScriptableObject
    {
        public bool isPlaceable;
        public Sprite[] textureVariants;
    }
}
