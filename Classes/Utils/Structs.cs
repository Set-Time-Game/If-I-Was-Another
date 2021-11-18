using System;
using Classes.World;
using UnityEngine;
using UnityEngine.GameFoundation;
using static Classes.Utils.Flags;

namespace Classes.Utils
{
    public static class Structs
    {
        [Serializable]
        public struct TexturesLayers
        {
            public string name;
            public Color color;
            //public float heightFrom;
            //public float heightTo;
            public float height;
            public ObjectType type;
        }
        
        [Serializable]
        public struct EnemyTag
        {
            public string name;
            public int hp;
        }
        
        [Serializable]
        public struct Button
        {
            public AttackType type;

            //TODO: change to Addressables
            public Sprite background;
            public Sprite handle;
            public Sprite button;
        }
        
        [Serializable]
        public struct Variable
        {
            public float chanceOfSpawn;
            
            public string id;
            public string name;
            
            public bool canPlacing;
            public bool hasCollision;
            
            public ObjectType type;
            public Vector2 size;
            public Views[] viewsArray;
            
            public Sprite[] texturesArray;
            public IGenerable Instance;
        }
        
        [Serializable]
        public struct Resource
        {
            public string id;
            public string name;
            public float chanceOfDrop;
            public Sprite texture;
        }

        [Serializable]
        public struct Views
        {
            public Sprite outlineTexture;
            public Sprite defaultTexture;
            public Sprite pickedTexture;
            public Resource[] resourcesArray;
            public GameObject spawnableMob;
        }
        
        //TODO: make it on addressables
        [Serializable]
        public struct JoystickModesViews
        {
            public States state;
            public AttackType type;

            public Sprite backgroundImage;
            public Sprite handleImage;
        }
    }
}