using System.Collections.Generic;
using Creatures.Model.Data;
using Creatures.Model.Definitions;
using UI.Windows;
using UnityEngine;

namespace UI.Hud.BigInventory
{
    public class BigInventoryController : AnimatedWindow
    {
        [SerializeField] private GameObject _slotContainer;

        public readonly List<BigInventorySlotWidget> _inventorySlots = new List<BigInventorySlotWidget>();
        private GameSession _session;


        protected override void Start()
        {
            base.Start();
            _session = FindObjectOfType<GameSession>();
            GetList();
            FillCollection();
        }


        private void GetList()
        {
            for (int i = 0; i < _slotContainer.transform.childCount; i++)
            {
                var child = _slotContainer.transform.GetChild(i);
                _inventorySlots.Add(child.GetComponent<BigInventorySlotWidget>());
            }
        }


        private void FillCollection()
        {
            var inventory = _session.Data.Inventory.GetAll();
            for (int t = 0; t < _inventorySlots.Count; t++)
            {
                for (int i = 0; i < inventory.Length; i++)
                {
                    if (i == t)
                    {
                        var def = DefsFacade.I.Items.Get(inventory[i].Id);
                        _inventorySlots[t].Icon.sprite = def.Icon;
                        _inventorySlots[t].Value.text = inventory[i].Value.ToString();
                        _inventorySlots[t].ActivateSlot();
                    }
                }
            }
        }


        public void SetData(InventoryItemData item, int index)
        {
            var def = DefsFacade.I.Items.Get(item.Id);
        }
    }
}
