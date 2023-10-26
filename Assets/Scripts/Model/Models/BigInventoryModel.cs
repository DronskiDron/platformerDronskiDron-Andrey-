using System;
using UI.Hud.BigInventory;

namespace Creatures.Model.Data.Models
{
    public class BigInventoryModel : IDisposable
    {
        private readonly PlayerData _data;

        public BigInventorySlotWidget[] _bigInventory;
        
        public BigInventoryModel(PlayerData data)
        {
            _data = data;
        }

        public void Dispose()
        {

        }
    }
}
