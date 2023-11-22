using System;
using Extensions;
using UI.Hud.BigInventory;

namespace Creatures.Model.Data.Models
{
    public class BigInventoryModel : IDisposable
    {
        private readonly PlayerData _data;
        public bool BigInventoryOnceWasFilled = false;

        public BigInventoryModel(PlayerData data)
        {
            _data = data;
            InitBigInventoryData(_data.Inventory.GetBigInventoryData(), _data.Inventory.GetAll());
            _data.Inventory.RenewInventory();
        }


        public InventoryItemData[] InitBigInventoryData(InventoryItemData[] slotDataArray, InventoryItemData[] itemDataArray)
        {
            for (int i = 0; i < slotDataArray.Length; i++)
            {
                for (int t = 0; t < itemDataArray.Length; t++)
                {
                    if (t > i) break;
                    if (t == i)
                    {
                        slotDataArray[i]
                        .SetId(itemDataArray[t])
                        .SetValue(itemDataArray[t]);
                    }
                }
            }
            return slotDataArray;
        }


        public BigInventorySlotWidget[] FillSlotArray(BigInventorySlotWidget[] slotArray, InventoryItemData[] dataArray)
        {
            for (int i = 0; i < slotArray.Length; i++)
            {
                for (int t = 0; t < dataArray.Length; t++)
                {
                    if (t > i) break;
                    if (t == i)
                    {
                        slotArray[i]
                        .SetId(dataArray[t])
                        .SetIcon(dataArray[t])
                        .SetText(dataArray[t])
                        .SetValue(dataArray[t]);
                    }
                }
            }
            return slotArray;
        }


        public InventoryItemData[] RenewBigInventoryData(InventoryItemData[] dataArray, BigInventorySlotWidget[] slotArray)
        {
            for (int i = 0; i < dataArray.Length; i++)
            {
                for (int t = 0; t < slotArray.Length; t++)
                {
                    if (t > i) break;
                    if (t == i)
                    {
                        dataArray[i]
                        .SetId(slotArray[t])
                        .SetValue(slotArray[t]);
                    }
                }
            }
            return dataArray;
        }


        public void SubscribeSlotEvents(BigInventorySlotWidget[] slotArray, Action action)
        {
            for (int i = 0; i < slotArray.Length; i++)
            {
                slotArray[i].Onchanged += action;
            }
        }


        public void UnSubscribeSlotEvents(BigInventorySlotWidget[] slotArray, Action action)
        {
            for (int i = 0; i < slotArray.Length; i++)
            {
                slotArray[i].Onchanged -= action;
            }
        }


        public void Dispose()
        {

        }
    }
}
