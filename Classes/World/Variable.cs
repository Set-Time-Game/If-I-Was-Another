using System;
using System.Collections.Generic;
using Classes.Entities.Enemies;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Classes.World
{
    [Serializable]
    public struct Variable
    {
        public sbyte id;
        public ObjectType type;
        public bool canPlacing;
        public bool hasCollision;
        public IGenerable Instance;
        public Vector2 size;
        public Views[] viewsArray;
        public Sprite[] texturesArray;
    }

    [Serializable]
    public struct Views
    {
        public Sprite outlineTexture;
        public Sprite defaultTexture;
        public Sprite pickedTexture;
        public sbyte[] resourcesArray;
        public GameObject spawnableMob;
    }

    [Serializable]
    public enum ObjectType
    {
        Ground,
        Obstacle,
        ResourceSource,
        Deco,
        Spawner
    }
}