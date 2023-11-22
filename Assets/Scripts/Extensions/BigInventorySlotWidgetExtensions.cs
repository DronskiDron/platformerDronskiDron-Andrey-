using Creatures.Model.Data;
using Creatures.Model.Definitions;
using UI.Hud.BigInventory;

namespace Extensions
{
    public static class BigInventorySlotWidgetExtensions
    {
        public static BigInventorySlotWidget SetId(this BigInventorySlotWidget slotWidget, InventoryItemData slotData)
        {
            slotWidget.Id = slotData.Id;
            return slotWidget;
        }


        public static BigInventorySlotWidget SetIcon(this BigInventorySlotWidget slotWidget, InventoryItemData slotData)
        {
            var def = DefsFacade.I.Items.Get(slotData.Id);
            slotWidget.Icon.sprite = def.Icon;
            return slotWidget;
        }


        public static BigInventorySlotWidget SetText(this BigInventorySlotWidget slotWidget, InventoryItemData slotData)
        {
            slotWidget.TextValue.text = slotData.Value.ToString();
            return slotWidget;
        }


        public static BigInventorySlotWidget SetValue(this BigInventorySlotWidget slotWidget, InventoryItemData slotData)
        {
            slotWidget.Value = slotData.Value;
            return slotWidget;
        }
    }
}
