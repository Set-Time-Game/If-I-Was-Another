using UnityEngine;

namespace Interfaces
{
    public interface IHighlightable
    {
        public Sprite OutlineTexture { get; }
        public Sprite DefaultTexture { get; }
        public void SetHighlight(bool enable);
    }
}