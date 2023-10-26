using Creatures.Model.Data;
using Creatures.Model.Definitions;
using UI.Windows;
using UnityEngine;

namespace UI.Hud.BigInventory
{
    public class BigInventoryController : AnimatedWindow
    {
        [SerializeField] private BigInventorySlotWidget[] _slots;
        public InventorySlotData[] _inventorySlots = new InventorySlotData[12];

        private GameSession _session;


        protected override void Start()
        {
            base.Start();
            _session = FindObjectOfType<GameSession>();

            if (_session.Data.Inventory.BigInventoryOnceWasFilled == false)
            {
                FillCollection();
            }
            else
            {
                RenewSlotsInfo();
            }
        }


        private void FillCollection()
        {
            var inventory = _session.Data.Inventory.GetAll();

            for (int t = 0; t < _slots.Length; t++)
            {
                for (int i = 0; i < inventory.Length; i++)
                {
                    if (i == t && inventory[i].Value > 0)
                    {
                        var def = DefsFacade.I.Items.Get(inventory[i].Id);
                        _slots[t].Id = def.Id;
                        _slots[t].Icon.sprite = def.Icon;
                        _slots[t].TextValue.text = inventory[i].Value.ToString();
                        _slots[t].Value = inventory[i].Value;
                        _slots[t].ActivateSlot();

                    }
                }
            }
            FillInventSlot();
            _session.Data.Inventory.FillBigInventory(_inventorySlots);
            _session.Data.Inventory.BigInventoryOnceWasFilled = true;
        }


        private void OnDestroy()
        {
            FillInventSlot();
            _session.Data.Inventory.FillBigInventory(_inventorySlots);
        }


        private void RenewSlotsInfo()
        {
            var inventoryDataArray = _session.Data.Inventory.GetBigInventory();
            for (int i = 0; i < _slots.Length; i++)
            {
                for (int t = 0; t < inventoryDataArray.Length; t++)
                {
                    if (i == t)
                    {
                        _slots[i].Id = inventoryDataArray[t].Id;
                        _slots[i].Icon.sprite = inventoryDataArray[t].Sprite;
                        _slots[i].TextValue.text = inventoryDataArray[t].TextValue;
                        _slots[i].Value = inventoryDataArray[t].Value;
                        if (_slots[t].Icon != null)
                            _slots[t].ActivateSlot();
                    }

                }
            }
        }


        private void FillInventSlot()
        {
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                for (int t = 0; t < _slots.Length; t++)
                {
                    if (i == t)
                    {
                        _inventorySlots[i].Id = _slots[t].Id;
                        _inventorySlots[i].Sprite = _slots[t].Icon.sprite;
                        _inventorySlots[i].TextValue = _slots[t].TextValue.text;
                        _inventorySlots[i].Value = _slots[t].Value;
                    }
                }
            }
        }
    }
}
