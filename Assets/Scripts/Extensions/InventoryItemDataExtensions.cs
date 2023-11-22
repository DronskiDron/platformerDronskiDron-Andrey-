using Creatures.Model.Data;
using UI.Hud.BigInventory;

namespace Extensions
{
    public static class InventoryItemDataExtensions
    {
        public static InventoryItemData SetId(this InventoryItemData inventorySlotData, InventoryItemData inventoryItemData)
        {
            inventorySlotData.Id = inventoryItemData.Id;
            return inventorySlotData;
        }
        public static InventoryItemData SetId(this InventoryItemData inventorySlotData, BigInventorySlotWidget slotWidget)
        {
            inventorySlotData.Id = slotWidget.Id;
            return inventorySlotData;
        }


        public static InventoryItemData SetValue(this InventoryItemData inventorySlotData, InventoryItemData inventoryItemData)
        {
            inventorySlotData.Value = inventoryItemData.Value;
            return inventorySlotData;
        }
        public static InventoryItemData SetValue(this InventoryItemData inventorySlotData, BigInventorySlotWidget slotWidget)
        {
            inventorySlotData.Value = slotWidget.Value;
            return inventorySlotData;
        }
    }
}
