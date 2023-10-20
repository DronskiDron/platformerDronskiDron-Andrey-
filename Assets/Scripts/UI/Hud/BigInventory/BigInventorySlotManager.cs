using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud.BigInventory
{
    public class BigInventorySlotManager : MonoBehaviour
    {
        [SerializeField] private List<SlotTransferWidget> _slotList = new List<SlotTransferWidget>();

        [HideInInspector] public Sprite InconstantItemSprite;
        [HideInInspector] public string InconstantItemValue;


    }
}
