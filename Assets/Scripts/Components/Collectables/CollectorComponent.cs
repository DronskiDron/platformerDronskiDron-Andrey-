using System.Collections.Generic;
using Creatures.Model.Data;
using UnityEngine;
using static Creatures.Model.Data.InventoryData;

namespace General.Components.Collectables
{
    public class CollectorComponent : MonoBehaviour, ICanAddInInventory
    {
        [SerializeField] private List<InventoryItemData> _items = new List<InventoryItemData>();


        public void AddInInventory(string id, int value)
        {
            _items.Add(new InventoryItemData(id) { Value = value });
        }


        public void DropInInventory()
        {
            var session = FindObjectOfType<GameSession>();

            foreach (var inventoryItemData in _items)
            {
                session.Data.Inventory.Add(inventoryItemData.Id, inventoryItemData.Value);
            }

            _items.Clear();
        }
    }
}
