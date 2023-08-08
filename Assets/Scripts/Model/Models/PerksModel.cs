﻿using System;
using Creatures.Model.Data.Properties;
using Creatures.Model.Definitions;
using Utils.Disposables;

namespace Creatures.Model.Data.Models
{
    public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty(default);

        /* private readonly CompositeDisposable _trash = new CompositeDisposable();
        public event Action OnChanged;

        public string Used => _data.Perks.Used.Value; */

        public PerksModel(PlayerData data)
        {
            _data = data;
            /* InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;

            _trash.Retain(_data.Perks.Used.Subscribe((x, y) => OnChanged?.Invoke()));
            _trash.Retain(InterfaceSelection.Subscribe((x, y) => OnChanged?.Invoke())); */
        }


       /*  public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        } */


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


       /*  internal bool CanBuy(string perkId)
        {
            var def = DefsFacade.I.Perks.Get(perkId);
            return _data.Inventory.IsEnough(def.Price);
        } */


        public void Dispose()
        {
           /*  _trash.Dispose(); */
        }
    }
}
