using System;
using Creatures.Model.Data;
using General.Components.TimeManipulation;
using UI.Windows;
using UnityEngine;

namespace UI.Hud.BigInventory
{
    public class BigInventoryController : AnimatedWindow
    {
        [SerializeField] private BigInventorySlotWidget[] _slots;
        [SerializeField] private DeleteFromInventoryComponent _throwComponent;

        private GameSession _session;


        protected override void Start()
        {
            base.Start();
            _session = FindObjectOfType<GameSession>();

            _session.BigInventory.SubscribeSlotEvents(_slots, RenewData);
            _throwComponent.OnChanged += RenewData;

            var slotDataArray = _session.Data.Inventory.GetBigInventoryData();
            var baseInventory = _session.Data.Inventory.GetAll();

            if (_session.BigInventory.BigInventoryOnceWasFilled == false)
            {
                _session.BigInventory.InitBigInventoryData(slotDataArray,
                baseInventory);
                _session.BigInventory.FillSlotArray(_slots, slotDataArray);
                ActivateSlots();
                _session.BigInventory.BigInventoryOnceWasFilled = true;
            }
            else
            {
                _session.BigInventory.FillSlotArray(_slots, baseInventory);
                ActivateSlots();
            }
            TimeManipulator.StopTime();
        }


        private void ActivateSlots()
        {
            foreach (var item in _slots)
            {
                item.ActivateSlot();
            }
        }


        private void RenewData()
        {
            var slotDataArray = _session.Data.Inventory.GetBigInventoryData();
            _session.BigInventory.RenewBigInventoryData(slotDataArray, _slots);
            _session.Data.Inventory.RenewInventory();
        }


        private void OnDestroy()
        {
            RenewData();
            _session.BigInventory.UnSubscribeSlotEvents(_slots, RenewData);
            _throwComponent.OnChanged -= RenewData;
            TimeManipulator.RunTimeNormal();
        }
    }
}
