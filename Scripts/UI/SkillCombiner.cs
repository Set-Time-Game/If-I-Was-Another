using System.Collections;
using System.Collections.Generic;
using Classes.Entities;
using Classes.Player;
using UnityEngine;

namespace UI
{
    public sealed class SkillCombiner : MonoBehaviour
    {
        [SerializeField] private string combo;
        [SerializeField] private Character player;
        [SerializeField] private GameObject buttons;
        [SerializeField] private FixedJoystick attackJoystick;
        
        private readonly WaitForSeconds _wipeTimeout = new WaitForSeconds(30);
        private Coroutine _wiperCoroutine;

        private void Awake()
        {
            Singleton = this;
        }

        private void Start()
        {
            player.OnWeaponSwap += MagePanelSwitch;

            if (player.AttackType == AttackType.Mage)
            {
                buttons.SetActive(false);
                if (_wiperCoroutine != null)
                    StopCoroutine(_wiperCoroutine);
            }
            else
            {
                _wiperCoroutine = StartCoroutine(ComboWiper());
            }
        }

        private void MagePanelSwitch(AttackType type)
        {
            if (type != AttackType.Mage)
            {
                if (buttons.activeSelf)
                    buttons.SetActive(false);

                return;
            }

            if (!buttons.activeSelf)
                buttons.SetActive(true);

        }

        public void OnButton(string buttonName)
        {
            if (_wiperCoroutine != null)
                StopCoroutine(_wiperCoroutine);

            combo += $"{buttonName}, ";
            _wiperCoroutine = StartCoroutine(ComboWiper());
        }

        private IEnumerator ComboWiper()
        {
            while (true)
            {
                yield return _wipeTimeout;
                combo = "";
            }
        }

        public static SkillCombiner Singleton { get; set; }
    }
}