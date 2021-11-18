using System;
using System.Collections.Generic;
using System.Linq;
using Classes.Player;
using Classes.World;
using Interfaces;
using Saves;
using UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.GameFoundation;
using UnityEngine.GameFoundation.DefaultLayers.Persistence;
using World;
using static Classes.Utils.Flags;
using static Classes.Utils.Utils;
using static Classes.Utils.Structs;

namespace Player
{
    public sealed class PlayerCollector : Collector
    {
        [SerializeField] private Character player;
        [SerializeField] private FixedJoystick attackJoystick;
        [SerializeField] private JoystickModesViews[] attackJoystickModes;
        private ICollectable _closestResource;

        private void Start()
        {
            player.onCollectingEvent += Collect;
            attackJoystick.OnPointerUpEvent += PointerUp;
        }

        private void FixedUpdate()
        {
            if (Resources.Count > 0)
            {
                var closest = ClosestFrom(Resources, transform.position);

                if (closest == _closestResource) return;

                closest.SetHighlight(true);
                _closestResource?.SetHighlight(false);
                _closestResource = closest;

                if (Enemies.Count < 1 && attackJoystick.mode != FixedJoystick.JoystickModes.Collect)
                    ChangeTo(States.Collecting, FixedJoystick.JoystickModes.Collect);
            }

            if (Enemies.Count < 1 || attackJoystick.mode == FixedJoystick.JoystickModes.Attack) return;

            ChangeTo(player.AttackType, FixedJoystick.JoystickModes.Attack);
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (Resources.Count < 1) return;

            ClosestFrom(Resources, transform.position)?.SetHighlight(true);

            if (Enemies.Count < 1)
                ChangeTo(States.Collecting, FixedJoystick.JoystickModes.Collect);
            else
                ChangeTo(player.AttackType, FixedJoystick.JoystickModes.Attack);
        }

        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
            if (Resources.Count > 0) return;

            _closestResource?.SetHighlight(false);
            ChangeTo(player.AttackType, FixedJoystick.JoystickModes.Attack);
        }

        private void Change(JoystickModesViews views, FixedJoystick.JoystickModes mode)
        {
            attackJoystick.backgroundImage.sprite = views.backgroundImage;
            attackJoystick.handleImage.sprite = views.handleImage;
            attackJoystick.mode = mode;
        }

        private void ChangeTo(AttackType type, FixedJoystick.JoystickModes mode)
            => Change(attackJoystickModes.First(x => x.type == type), mode);

        private void ChangeTo(States state, FixedJoystick.JoystickModes mode)
            => Change(attackJoystickModes.First(x => x.state == state), mode);

        private void PointerUp(PointerEventData data)
        {
            if (player.State != States.None || _closestResource == null
                                            || Enemies.Count > 0 || Resources.Count < 1
                                            || _closestResource.Resources.Length < 1) return;

            Flip(player.spriteRenderer, -(player.Transform.position - _closestResource.Transform.position),
                player.animator);
            player.ChangeState(States.Collecting);
        }

        public void Collect()
        {
            var resources = _closestResource.Collect();
            if (resources.Length < 1) return;

            foreach (var resource in resources)
            {
                //return;
                var catalogItem = GameFoundationSdk.catalog.Find<InventoryItemDefinition>(resource.id);
                var item = GameFoundationSdk.inventory.CreateItem(catalogItem);
                var biome = _closestResource.GameObject.GetComponentInParent<Ground>()
                    .GetComponentInParent<BiomeGenerator>();
                InventoryManager.Singleton.AddItem(biome.resourcesVariables.First(x => x.id == catalogItem.key).texture);
            }
        }
    }
}