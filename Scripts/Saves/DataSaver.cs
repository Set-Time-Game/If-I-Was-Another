using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.GameFoundation;
using UnityEngine.GameFoundation.Data;
using UnityEngine.Promise;

namespace Saves
{
    public static class DataSaver
    {
        public static void LoadInventory()
        {
            var data = JsonConvert.DeserializeObject<InventoryManagerData>(PlayerPrefs.GetString("inventory"));
            foreach (var item in data.items)
            {
                var catalogItem = GameFoundationSdk.catalog.Find<CatalogItem>(item.definitionKey);
                GameFoundationSdk.inventory.CreateItem((InventoryItemDefinition) catalogItem);
            }
        }
        public static void SaveInventory()
        {
            var data = ((IInventoryDataLayer) GameFoundationSdk.dataLayer).GetData();
            PlayerPrefs.SetString("inventory", JsonConvert.SerializeObject(data));
        }

        public static InventoryItem[] GetInventoryItemsKeys()
        {
            var items = new List<InventoryItem>();
            GameFoundationSdk.inventory.GetItems(items);
            return items.ToArray();
        }
    }
}