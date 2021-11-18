using Interfaces;
using UnityEngine;
using static Classes.Utils.Structs;

namespace Classes.World
{
    public abstract class HighlightableProp : Prop, IHighlightable
    {
        [SerializeField] protected Collider2D collectCollider;
        
        protected Sprite _defaultTexture;
        protected Sprite _outlineTexture;
        
        public virtual void SetHighlight(bool enable)
        {
            if (Variety.viewsArray.Length <= 0 || !collectCollider.enabled || !OutlineTexture || !DefaultTexture) return;

            spriteRenderer.sprite = enable && OutlineTexture ? OutlineTexture : DefaultTexture;
        }
        
        public virtual Sprite OutlineTexture => _outlineTexture;
        public virtual Sprite DefaultTexture => _defaultTexture;
    }
}