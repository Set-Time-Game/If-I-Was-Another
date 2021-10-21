using System;
using System.Collections.Generic;
using Classes.Entities;
using Classes.Player;
using UnityEngine;

namespace Classes.UI
{
    [Serializable]
    public struct Button
    {
        public AttackType type;
        //TODO: change to Addressables
        public Sprite background;
        public Sprite handle;
        public Sprite button;
    }
}