using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Classes.World;
using UnityEngine;
using UnityEngine.UI;
using static Classes.Utils.Flags;
using static Classes.Utils.Structs;
using Random = UnityEngine.Random;

namespace World
{
    public sealed class BiomeGenerator : MonoBehaviour
    {
        public bool isGenerated;

        public TexturesLayers[] objectsLayers;

        [Space]
        public Resource[] resourcesVariables;
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

        public readonly Dictionary<Vector2, Variable> MapResourceSourceVariable =
            new Dictionary<Vector2, Variable>();

        public readonly Dictionary<Vector2, Variable> MapSpawnerVariable =
            new Dictionary<Vector2, Variable>();
        
        private void Start()
        {
            var world = Classes.World.World.Singleton;
            var map = world.Map;
            var mapDeco = world.MapDeco;
            var zeroPoint = world.zeroPoint;

            var ground = GetComponentsInChildren<IGenerable>(true)
                .Where(x => x.Variety.type == ObjectType.Ground);
            SetTo(ground, map, zeroPoint, false);

            //yield break;
            
            var objects = GetComponentsInChildren<IGenerable>().Where(x => x.Variety.type != ObjectType.Ground).ToArray();
            foreach (var obj in objectsLayers.Where(x => x.type != ObjectType.Ground))
            {
                SetTo(objects.Where(x => x.Variety.type == obj.type), mapDeco, zeroPoint);
            }
            

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
                if (!target.GameObject) continue;

                if (layer.type != target.Variety.type)
                    layer = objectsLayers.First(l => l.type == type);

                var position = target.Transform.position;
                var level = map[(int) -(zeroPoint.x - position.x), (int) (zeroPoint.y - position.y)];

                //if (layer.heightFrom <= level && layer.heightTo >= level &&
                if (layer.height >= level &&
                    !MapObstacleVariable.ContainsKey(position) &&
                    !MapResourceSourceVariable.ContainsKey(position))
                {
                    if (layer.chanceOfSpawn == 0 || layer.chanceOfSpawn >= Random.value)
                    {
                        target.Generate(out var variable);
                        SaveVariable(ref variable);

                        if (variable.size != Vector2.zero)
                        {
                            for (var x = variable.size.x; x >= -variable.size.x; x--)
                            {
                                for (var y = variable.size.y; y >= -variable.size.y; y--)
                                {
                                    var volume = new Vector2(x, y);
                                    var intVolume = new Vector2Int((int) x, (int) y);
                                    var pos = (Vector2) position + intVolume;

                                    if ((pos) == (Vector2) position) continue;

                                    if (!MapGroundVariable.TryGetValue(pos, out var ground) || !ground.canPlacing)
                                        continue;

                                    intVolume = new Vector2Int(Mathf.Abs(intVolume.x), Mathf.Abs(intVolume.x));
                                    ground.Instance.GameObject.GetComponent<Ground>()
                                        .DisablePlacing(volume, variable.size, intVolume);

                                }
                            }
                        }
                        continue;
                    }
                }

                if (!destroyIfLess || !target.GameObject) continue;

                Destroy(target.GameObject);
            }
        }

        private void SaveVariable(ref Variable variable)
        { var position = variable.Instance.Transform.position;

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
    }
}