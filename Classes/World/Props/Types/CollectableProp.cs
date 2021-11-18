using Interfaces;
using UnityEngine;
using static Classes.Utils.Structs;

namespace Classes.World
{
    public abstract class CollectableProp : HighlightableProp, ICollectable
    {
        protected Sprite _pickedTexture;
        protected Resource[] _resources;
        
        public virtual Resource[] Collect()
        {
            return _resources;
        }
        
        public virtual Resource[] Resources => _resources;
        public virtual Sprite PickedTexture => _pickedTexture;
    }
}