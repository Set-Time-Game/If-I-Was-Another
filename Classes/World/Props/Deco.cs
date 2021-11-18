using System;
using UnityEngine;
using World;
using static Classes.Utils.Structs;
using Random = UnityEngine.Random;

namespace Classes.World
{
    [Serializable]
    public sealed class Deco : Prop
    {
        public override void Generate(out Variable variable)
        {
            CreateVariable(biome.decoVariablesList, out variable);
            var texture = variable.texturesArray[Random.Range(0, variable.texturesArray.Length)];

            spriteRenderer.sprite = texture;

            variety = variable;
            variety.texturesArray = new[] {texture};

            variety.Instance = this;

            variable = variety;
            base.Generate(out variable);
        }
    }
}