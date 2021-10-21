using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Classes.Entities;
using Classes.World;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace World
{
    public sealed class BiomeGenerator : MonoBehaviour
    {
        public bool isGenerated;

        public TexturesLayers[] objectsLayers;

        [Space] 
        public Variable[] resourceVariablesList;
        public Variable[] groundVariablesList;
        public Variable[] resourceSourceVariablesList;
        public Variable[] obstacleVariablesList;
        public Variable[] spawnerVariablesList;
        public Variable[] decoVariablesList;

        public readonly Dictionary<Vector2, Variable> MapDecoVariable =
            new Dictionary<Vector2, Variable>();

        public readonly Dictionary<Vector2, Variable> MapGroundVariable =
            new Dictionary<Vector2, Variable>();

        public readonly Dictionary<Vector2, Variable> MapObstacleVariable =
            new Dictionary<Vector2, Variable>();
        
        public readonly Dictionary<Vector2, Variable> MapSpawnerVariable =
            new Dictionary<Vector2, Variable>();

        public readonly Dictionary<Vector2, Variable> MapResourceSourceVariable =
            new Dictionary<Vector2, Variable>();

        private void Start()
        {
            var world = Classes.World.World.Singleton;
            var map        = world.Map;
            var mapDeco    = world.MapDeco;
            var zeroPoint = world.zeroPoint;

            var objects = GetComponentsInChildren<IGenerable>();
            var ground    = objects.Where(x => x.Variety.type == ObjectType.Ground);
            var obstacles = objects.Where(x => x.Variety.type == ObjectType.Spawner || x.Variety.type == ObjectType.Obstacle);
            var resources = objects.Where(x => x.Variety.type == ObjectType.ResourceSource);
            var deco      = objects.Where(x => x.Variety.type == ObjectType.Deco);

            SetTo(ground,         map,     zeroPoint, false);
            SetTo(obstacles,      mapDeco, zeroPoint);
            SetTo(resources,      mapDeco, zeroPoint);
            SetTo(deco,           mapDeco, zeroPoint);

            isGenerated = true;
        }

        //TODO: split to IJob with burst and placer that deletes every that object not in range and after generates others
        private void SetTo(IEnumerable<IGenerable> targets, float[,] map, Vector2 zeroPoint, bool destroyIfLess = true)
        {
            var generables = targets as IGenerable[] ?? targets.ToArray();
            var type = generables.First().Variety.type;
            var layer = objectsLayers.First(l => l.type == type);
            
            foreach (var target in generables)
            {
                if (layer.type != target.Variety.type)
                    layer = objectsLayers.First(l => l.type == type);
                
                var position = target.Transform.position;
                var level = map[(int) -(zeroPoint.x - position.x), (int) (zeroPoint.y - position.y)];

                if (layer.heightFrom <= level && layer.heightTo >= level &&
                    !MapObstacleVariable.ContainsKey(position) &&
                    !MapResourceSourceVariable.ContainsKey(position))
                {
                    target.Generate(out var variable);
                    SaveVariable(ref variable);
                    continue;
                }

                if (!destroyIfLess || !target.GameObject) continue;

                Destroy(target.GameObject);
            }
        }

        private void SaveVariable(ref Variable variable)
        {
            var position = variable.Instance.Transform.position;
            
            switch (variable.type)
            {
                case ObjectType.Ground:
                    MapGroundVariable[position] = variable;
                    break;
                case ObjectType.Obstacle:
                    MapObstacleVariable[position] = variable;
                    break;
                case ObjectType.Spawner:
                    MapSpawnerVariable[position] = variable;
                    break;
                case ObjectType.ResourceSource:
                    MapResourceSourceVariable[position] = variable;
                    break;
                case ObjectType.Deco:
                    MapDecoVariable[position] = variable;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(variable.type), variable.type, null);
            }
        }

        public IEnumerable<Vector2> GenerateDirections(Vector2 pos)
        {
            var x = pos.x;
            var y = pos.y;

            return new[]
            {
                new Vector2(x + 1, y),
                new Vector2(x - 1, y),

                new Vector2(x, y + 1),
                new Vector2(x, y - 1),

                new Vector2(x + 1, y + 1),
                new Vector2(x - 1, y - 1),

                new Vector2(x + 1, y - 1),
                new Vector2(x - 1, y + 1)
            };
        }

        [Serializable]
        public struct TexturesLayers
        {
            public string name;
            public float heightFrom;
            public float heightTo;
            public ObjectType type;
        }
    }
}