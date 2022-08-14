using System;
using Leopotam.Ecs;
using Systems.General;
using Systems.General.Control;
using Types.Classes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Systems
{
    public class EcsWorldInitializer : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _systems;

        [SerializeField]
        private MapData mapData;
        [SerializeField]
        private InventorySystem inventorySystem;
        [SerializeField]
        private ControlsData controlsData;
        [SerializeField]
        private PlayerController playerController;
        [SerializeField]
        private Player player;

        private void Awake()
        {
            Application.targetFrameRate = 60;

            //Random.InitState((int) DateTime.UtcNow.Ticks);
            Random.InitState(1);
            var worldState = new WorldStateSystem();
            inventorySystem.WorldState = worldState;
            _world = new EcsWorld();
            _systems = new EcsSystems(_world)
                .Inject(mapData)
                .Inject(player)
                .Inject(controlsData)
                .Add(new GeneratorSystem())
                .Add(worldState)
                .Add(inventorySystem);
            
            _systems.Inject(_systems);
        }

        private void Start()
        {
            _systems.Init();
            
            GC.Collect();
        }

        private void Update() => _systems.Run();

        private void OnDestroy()
        {
            _systems.Destroy();
            _systems = null;
            _world.Destroy();
            _world = null;
            
            GC.Collect();
        }
    }
}
