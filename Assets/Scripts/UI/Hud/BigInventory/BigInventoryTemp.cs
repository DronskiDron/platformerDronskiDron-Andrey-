using Creatures.Model.Definitions.Repository.Items;
using UnityEngine;

namespace UI.Hud.BigInventory
{
    public class BigInventoryTemp : MonoBehaviour
    {
        [HideInInspector][InventoryId] public string InconstantId;
        [HideInInspector] public Sprite InconstantItemSprite;
        [HideInInspector] public string InconstantItemTextValue;
        [HideInInspector] public int InconstantValue;
        [HideInInspector] public int SlotIndex;


        public void RenewTemp(BigInventorySlotWidget slotWidget)
        {
            InconstantId = slotWidget.Id;
            InconstantItemSprite = slotWidget.Icon.sprite;
            InconstantItemTextValue = slotWidget.TextValue.text;
            InconstantValue = slotWidget.Value;
        }


        public void RenewFromTemp(BigInventorySlotWidget slotWidget)
        {
            slotWidget.Id = InconstantId;
            slotWidget.Icon.sprite = InconstantItemSprite;
            slotWidget.TextValue.text = InconstantItemTextValue;
            slotWidget.Value = InconstantValue;
        }



    }
}
