using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Events;

namespace Types.Classes
{
    public class PlayerCollector : MonoBehaviour
    {
        public List<GroundPart> Collectables;
        public LayerMask CollectableLayer;

        public UnityEvent<GroundPart, List<GroundPart>> CollectableAdd;
        public UnityEvent<GroundPart, List<GroundPart>> CollectableRemove;

        public void OnTriggerEnter2D(Collider2D other)
        {
            if ((1 << other.gameObject.layer) != CollectableLayer.value) return;
            var part = other.GetComponent<GroundPart>();
            Collectables.Add(part);
            CollectableAdd?.Invoke(part, Collectables);
        }
        
        public void OnTriggerExit2D(Collider2D other)
        {
            if ((1 << other.gameObject.layer) != CollectableLayer.value) return;
            var part = other.GetComponent<GroundPart>();
            Collectables.Remove(part);
            CollectableRemove?.Invoke(part, Collectables);
        }
    }
}