using System;
using System.Linq;
using Interfaces;
using UnityEngine;
using World;
using static Classes.Utils.Structs;
using Random = UnityEngine.Random;

namespace Classes.World
{
    [Serializable]
    public sealed class ResourceSource : CollectableProp
    {
        public override Resource[] Collect()
        {
            collectCollider.enabled = false;
            spriteRenderer.sprite = PickedTexture;

            return Resources;
        }
        
        public override void Generate(out Variable variable)
        {
            CreateVariable(biome.resourceSourceVariablesList, out variable);
            
            foreach (var i in
                GetComponentsInParent<Deco>()
                    .Select(x => x.gameObject)
                    .Where(x => x.transform.localPosition == transform.localPosition))
                Destroy(i);
            var texture = variable.viewsArray[Random.Range(0, variable.viewsArray.Length)];

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