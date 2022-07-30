using System;
using Types.Interfaces;
using UnityEngine;

namespace Types.Structs
{
    [Serializable]
    public enum PartType
    {
        None = 0,
        Small = 1,
        Big = 2,
        Struct = 4
    }
    
    [Serializable]
    public enum PartState
    {
        None = 0,
        Default = 1,
        Highlighted = 2,
        Picked = 4,
        Destroyed = 8
    }
    
    [Serializable]
    public struct GroundPartConfig : IVariant
    {
        public string name;
        public float chance;

        public bool hasCollision;
        public bool hasCollector;

        public PartType type;
        public PartState state;

        public GroundPartPairs[] pairsVariantas;
        public Sprite[] textureVariants;

        public float Chance => chance;
    }

    [Serializable]
    public struct GroundPartPairs
    {
        public Sprite mainTexture;
        public Sprite highlightedTexture;
        public Sprite pickedTexture;
    }
}