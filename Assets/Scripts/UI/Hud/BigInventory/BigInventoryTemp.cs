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



    }
}
