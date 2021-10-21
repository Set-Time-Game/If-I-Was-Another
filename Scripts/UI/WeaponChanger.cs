using System.Collections.Generic;
using Classes.Entities;
using Classes.Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public sealed class WeaponChanger : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] public AttackType buttonType;
        [SerializeField] private List<WeaponChanger> otherButtons;

        [SerializeField] private Character player;
        [SerializeField] private FixedJoystick attackJoystick;
        [SerializeField] private Classes.UI.Button defaultButton;
        [SerializeField] private Classes.UI.Button meleeButton;

        private void Start() => player.OnWeaponSwap += AttackSwapped;

        private void AttackSwapped(AttackType type)
        {
            if (type == buttonType)
            {
                image.sprite = meleeButton.button;
                attackJoystick.backgroundImage.sprite = defaultButton.background;
                attackJoystick.handleImage.sprite = defaultButton.handle;

                foreach (var button in otherButtons) button.image.sprite = button.defaultButton.button;
            }
            else if (type == AttackType.Melee)
            {
                image.sprite = defaultButton.button;
                attackJoystick.backgroundImage.sprite = meleeButton.background;
                attackJoystick.handleImage.sprite = meleeButton.handle;
            }
        }
    }
}