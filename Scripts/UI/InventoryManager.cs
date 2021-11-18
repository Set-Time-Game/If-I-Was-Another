using System;
using System.Linq;
using Classes.UI;
using UnityEngine;
using UnityEngine.GameFoundation;

namespace UI
{
    [Serializable]
    public class InventoryManager : MonoBehaviour
    {
        public Sprite lockedSprite;
        public Sprite defaultSprite;
        public static InventoryManager Singleton { get; private set; }
        [SerializeField] private InventoryManagerRow[] rows;

        private void Awake()
        {
            Singleton = this;
            //GetComponentInParent<GameObject>().SetActive(false);
            //gameObject.SetActive(false);
        }

        public void AddItem(Sprite item)
        {
            if (rows.Any(row => row.AddItem(item)))
            {
                return;
            }
        }
    }
}