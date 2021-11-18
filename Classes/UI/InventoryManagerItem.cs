using System;
using UnityEngine;
using UnityEngine.UI;

namespace Classes.UI
{
    [Serializable]
    public class InventoryManagerItem : MonoBehaviour
    {
        public int index;
        public Image socketImage;
        public Image itemImage;
        public TMPro.TMP_Text stackText;
    }
}