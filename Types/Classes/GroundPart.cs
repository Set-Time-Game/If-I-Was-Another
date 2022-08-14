using UnityEngine;

namespace Types.Classes
{
    public class GroundPart : MonoBehaviour
    {
        public byte Id;
        public byte GroundId;
        public SpriteRenderer spriteRenderer;

        public CapsuleCollider2D collisionCollider;
        public CircleCollider2D collectCollider;
    }
}