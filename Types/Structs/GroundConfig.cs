using System;
using Types.Interfaces;
using UnityEngine;

namespace Types.Structs
{
    [Serializable]
    public struct GroundConfig : IVariant
    {
        public string name;
        public float chance;

        public bool isPlaceable;
        public Sprite[] textureVariants;
        public float Chance => chance;
    }
}