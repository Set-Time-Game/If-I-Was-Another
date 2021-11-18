using System;
using System.Threading;
using UI;
using UnityEngine;

namespace Classes.UI
{
    [Serializable]
    public class InventoryManagerRow : MonoBehaviour
    {
        [SerializeField] private InventoryManagerItem[] items;
        private sbyte _itemsCount = 0;

        public bool AddItem(Sprite sprite)
        {
            if (_itemsCount >= items.Length)
                return false;

            var item = items[_itemsCount].itemImage;
            item.sprite = sprite;
            item.gameObject.SetActive(true);
            _itemsCount++;
            return true;
        }
        
        public void Lock()
        {
            foreach (var item in items)
            {
                item.socketImage.sprite = InventoryManager.Singleton.lockedSprite;
            }
        }
        
        public void Unlock()
        {
            foreach (var item in items)
            {
                item.socketImage.sprite = InventoryManager.Singleton.defaultSprite;
            }
        }

        public void OnItemClicked(InventoryManagerItem item)
        {
            if (item.index == -1) return;
        }
    }
}