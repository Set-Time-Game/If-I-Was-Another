using System;
using System.Collections;
using UnityEngine;
using static Classes.Utils.Flags;

namespace Classes.Entities
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        public new Rigidbody2D rigidbody;
        [SerializeField] public SpriteRenderer spriteRenderer;
        
        
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;
        public SpriteRenderer SpriteRenderer => spriteRenderer;
    }
}