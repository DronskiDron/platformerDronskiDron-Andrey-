using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Hud.BigInventory
{
    public class DeleteFromInventoryComponent : MonoBehaviour, IDropHandler
    {
        [SerializeField] private BigInventoryTemp _slotManager;

        public Action OnChanged;


        public void OnDrop(PointerEventData eventData)
        {
            var slotIndex = _slotManager.SlotIndex;
            var draggedObject = eventData.pointerDrag;
            if (draggedObject != null)
            {
                var transferWidget = draggedObject.GetComponent<SlotTransferWidget>();
                transferWidget?.ClearSlot();
                transferWidget?.DonorAfterDrug(transferWidget);
                OnChanged?.Invoke();
            }
        }
    }
}
