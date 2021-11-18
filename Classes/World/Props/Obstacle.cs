using System;
using System.Linq;
using UnityEngine;
using World;
using static Classes.Utils.Structs;
using Random = UnityEngine.Random;

namespace Classes.World
{
    [Serializable]
    public class Obstacle : BigProp
    {
        
        public override void Generate(out Variable variable)
        {
            CreateVariable(biome.obstacleVariablesList, out variable);
            var texture = variable.viewsArray[Random.Range(0, variable.viewsArray.Length)];

            if (texture.resourcesArray.Length < 1)
                Destroy(GetComponent<CircleCollider2D>());

            _defaultTexture = texture.defaultTexture;
            _outlineTexture = texture.outlineTexture;
            _pickedTexture = texture.pickedTexture;
            _resources = texture.resourcesArray;
            
            spriteRenderer.sprite = texture.defaultTexture;

            variety = variable;
            variety.viewsArray = new[] {texture};

            variety.Instance = this;

            variable = variety;
            
            base.Generate(out variable);
        }
    }
}