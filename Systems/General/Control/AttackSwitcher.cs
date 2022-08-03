
using System;
using Types.Classes;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.General.Control
{
    [RequireComponent(typeof(ControlButton), typeof(Image))]
    public class AttackSwitcher : MonoBehaviour
    {
        public Image Image;
        public Sprite Range;
        public Sprite Melee;

        public void OnAttackModeSwitch(AttackMode _, AttackMode mode)
            => Image.sprite = mode switch 
            {
                AttackMode.Melee => Range,
                AttackMode.Range => Melee,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
    }
}