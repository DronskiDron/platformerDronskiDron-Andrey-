using System;
using Creatures.Model.Data.Properties;
using Creatures.Model.Definitions;

namespace Creatures.Model.Data.Models
{
    public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty(default);


        public PerksModel(PlayerData data)
        {
            _data = data;
        }


        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var isEnoughResources = _data.Inventory.IsEnough(def.Price);

            if (isEnoughResources)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Perks.AddPerk(id);
            }
        }


        internal void UsePerk(string selected)
        {
            _data.Perks.Used.Value = selected;
        }


        internal bool IsUsed(string perkId)
        {
            return _data.Perks.Used.Value == perkId;
        }


        internal bool IsUnlocked(string perkId)
        {
            return _data.Perks.IsUnlocked(perkId);
        }


        public void Dispose()
        {

        }
    }
}
