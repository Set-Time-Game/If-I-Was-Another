using System;
using Classes.Entities;
using Classes.Player;
using Classes.World;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    public sealed class PlayerCollector : Collector
    {
        [SerializeField] private Character player;
        [SerializeField] private FixedJoystick attackJoystick;

        private IGenerable _closestResource;

        private void Start()
        {
            attackJoystick.OnPointerUpEvent += Collect;
        }

        private void FixedUpdate()
        {
            if (Resources.Count > 0)
            {
                var closest = Entity.ClosestFrom(Resources, transform.position);

                if (closest == _closestResource) return;

                closest.SetHighlight(true);
                _closestResource?.SetHighlight(false);
                _closestResource = closest;
            }

            if (Enemies.Count > 0)
            {
                
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (Resources.Count > 0)
            {
                
            }
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
            if (Resources.Count < 1)
            {
                _closestResource?.SetHighlight(false);
            }
        }

        private void Collect(PointerEventData data)
        {
            if (player.State != States.None || _closestResource == null
                || Enemies.Count > 0 || Resources.Count < 1 
                || _closestResource.Variety.viewsArray.Length < 1 || _closestResource.Variety.viewsArray[0].resourcesArray.Length < 1) return;

            player.ChangeState(States.Collecting);
        }
    }
}