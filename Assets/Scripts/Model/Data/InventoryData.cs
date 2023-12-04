using System;
using System.Collections.Generic;
using System.Linq;
using Creatures.Model.Definitions;
using Creatures.Model.Definitions.Items;
using Creatures.Model.Definitions.Repository;
using Creatures.Model.Definitions.Repository.Items;
using UnityEngine;

namespace Creatures.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();
        [SerializeField] private InventoryItemData[] _inventorySlots = new InventoryItemData[12];

        public delegate void OnInventoryChanged(string id, int value);
        public OnInventoryChanged OnChanged;


        public void Add(string id, int value)
        {
            if (value <= 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            var noneList = SearchEmptyItem();

            if (itemDef.HasTag(ItemTag.Stackable))
            {
                if (IsStackAlreadyExists(id))
                    AddToStack(id, value);
                if (!IsStackAlreadyExists(id) && noneList.Count != 0)
                    SettleStack(id, value, noneList[0]);
            }
            else
            {
                if (noneList.Count <= 0) return;
                AddNonStack(id, value, noneList[0]);
            }

            OnChanged?.Invoke(id, Count(id));
        }


        public InventoryItemData[] GetAll(params ItemTag[] tags)
        {
            var retValue = new List<InventoryItemData>();

            foreach (var item in _inventory)
            {
                var itemDef = DefsFacade.I.Items.Get(item.Id);
                var isAllRequirementsMet = tags.All(x => itemDef.HasTag(x));
                if (isAllRequirementsMet)
                    retValue.Add(item);
            }

            return retValue.ToArray();
        }


        private void SettleStack(string id, int value, int index)
        {
            var item = GetItem(id);
            if (item == null)
            {
                item = new InventoryItemData(id);
                _inventory[index] = item;
                item.Value += value;
            }
        }


        private bool IsStackAlreadyExists(string id)
        {
            var item = GetItem(id);
            if (item == null) return false;
            else return true;
        }


        private void AddToStack(string id, int value)
        {
            var item = GetItem(id);
            item.Value += value;
            Debug.Log("AddToStack");
        }


        private void AddNonStack(string id, int value, int index)
        {
            for (int i = 0; i < value; i++)
            {
                var item = new InventoryItemData(id) { Value = 1 };
                _inventory[index] = item;
            }
        }


        public void Remove(string id, int value)
        {
            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;
            var index = GetItemIndex(id);

            if (itemDef.HasTag(ItemTag.Stackable))
            {
                RemoveFromStack(id, value, index);
            }
            else
            {
                RemoveNonStack(id, value, index);
            }

            OnChanged?.Invoke(id, Count(id));
        }


        private void RemoveFromStack(string id, int value, int index)
        {
            var item = GetItem(id);
            if (item == null) return;

            item.Value -= value;

            if (item.Value <= 0)
            {
                _inventory[index] = new InventoryItemData("None");
                _inventory[index].Value = 0;
            }
        }


        private void RemoveNonStack(string id, int value, int index)
        {
            for (int i = 0; i < value; i++)
            {
                var item = GetItem(id);
                if (item == null) return;
                _inventory[index] = new InventoryItemData("None");
                _inventory[index].Value = 0;
            }
        }


        public void RemoveDefiniteItem(int index)
        {
            _inventory[index] = new InventoryItemData("None");
            _inventory[index].Value = 0;
        }


        public int Count(string id)
        {
            var count = 0;
            foreach (var item in _inventory)
            {
                if (item.Id == id)
                {
                    count += item.Value;
                }
            }
            return count;
        }


        public InventoryItemData GetItem(string id)
        {
            foreach (var itemData in _inventory)
            {
                if (itemData.Id == id)
                    return itemData;
            }
            return null;
        }


        public int GetItemIndex(string id)
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].Id == id)
                    return i;
            }
            return -1;
        }


        public bool IsEnough(params ItemWithCount[] items)
        {
            var joined = new Dictionary<string, int>();
            foreach (var item in items)
            {
                if (joined.ContainsKey(item.ItemId))
                    joined[item.ItemId] += item.Count;
                else
                    joined.Add(item.ItemId, item.Count);
            }

            foreach (var kvp in joined)
            {
                var count = Count(kvp.Key);
                if (count < kvp.Value) return false;
            }

            return true;
        }


        public InventoryItemData[] GetBigInventoryData()
        {
            return _inventorySlots;
        }


        public void RenewInventory()
        {
            _inventory.Clear();
            _inventory = _inventorySlots.ToList();
            OnChanged?.Invoke("None", 0);
        }


        private List<int> SearchEmptyItem()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < _inventory.Count; i++)
            {
                if (_inventory[i].Id == "None")
                {
                    list.Add(i);
                    break;
                }
            }
            return list;
        }
    }


    [Serializable]
    public class InventoryItemData
    {
        [InventoryId] public string Id;
        public int Value;

        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}
