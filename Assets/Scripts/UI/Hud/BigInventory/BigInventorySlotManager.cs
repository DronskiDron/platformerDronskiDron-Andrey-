using System.Collections.Generic;
using Creatures.Model.Definitions.Repository.Items;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud.BigInventory
{
    public class BigInventorySlotManager : MonoBehaviour
    {
        [HideInInspector][InventoryId] public string InconstantId;
        [HideInInspector] public Sprite InconstantItemSprite;
        [HideInInspector] public string InconstantItemTextValue;
        [HideInInspector] public int InconstantValue;



    }
}
