using UnityEngine;
using static Classes.Utils.Structs;

namespace Interfaces
{
    public interface ICollectable : IHighlightable, IEntity
    {
        public Sprite PickedTexture { get; }
        public Resource[] Resources { get; }
        public Resource[] Collect();
    }
}