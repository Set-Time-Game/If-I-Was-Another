using UnityEngine;

namespace Types.Classes
{
    public class GroundPart : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;

        public CapsuleCollider2D collisionCollider;
        public CircleCollider2D collectCollider;
    }
}