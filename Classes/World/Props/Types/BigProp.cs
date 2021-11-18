using System;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using Interfaces;
using UnityEngine;
using static Classes.Utils.Structs;

namespace Classes.World
{
    public abstract class BigProp : Prop, ICollectable
    {

        public override void Generate(out Variable variable)
        {
            base.Generate(out variable);
            
            if (variable.size != Vector2.zero)
            {
                var ground = biome.MapGroundVariable[Transform.position]
                    .Instance.GameObject
                    .GetComponent<Ground>();
                foreach (var i in ground.GetComponentsInChildren<Prop>().Except(new []{(Prop) this, ground}))
                {
                    Destroy(i.GameObject);
                }
            }
            
            if (!_pickedTexture || !_outlineTexture || _resources == null || _resources.Length < 1)
                Destroy(collectCollider);
        }
        
        protected Sprite _pickedTexture;
        protected Resource[] _resources;
        
        public virtual Resource[] Collect()
        {
            collectCollider.enabled = false;
            spriteRenderer.sprite = PickedTexture;
            return _resources;
        }
        
        public virtual Resource[] Resources => _resources;
        public virtual Sprite PickedTexture => _pickedTexture;
        
        [SerializeField] protected Collider2D collectCollider;
        
        protected Sprite _defaultTexture;
        protected Sprite _outlineTexture;
        
        public virtual void SetHighlight(bool enable)
        {
            if (!collectCollider || Variety.viewsArray.Length <= 0 || !collectCollider.enabled || !OutlineTexture || !DefaultTexture) return;

            spriteRenderer.sprite = enable && OutlineTexture ? OutlineTexture : DefaultTexture;
        }
        
        public virtual Sprite OutlineTexture => _outlineTexture;
        public virtual Sprite DefaultTexture => _defaultTexture;
    }
}