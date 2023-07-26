using System;
using Creatures.Model.Data.Properties;
using UnityEngine;

namespace Creatures.Model.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;

        private static int _defaultValue = 0;


        public IntProperty Hp = new IntProperty(_defaultValue);
        public PerksData Perks = new PerksData();
        public InventoryData Inventory => _inventory;


        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
}

