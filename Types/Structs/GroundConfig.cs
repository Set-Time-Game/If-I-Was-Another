using System;
using Types.Interfaces;
using UnityEngine;
using Variants.Ground;

namespace Types.Structs
{
    [Serializable]
    public struct GroundConfig : IVariant
    {
        public string name;
        public float chance;
        public GroundBase config;
        public float Chance => chance;
    }
}