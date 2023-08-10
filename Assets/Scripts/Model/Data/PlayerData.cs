﻿using System;
using Creatures.Model.Data.Properties;
using UnityEngine;

namespace Creatures.Model.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private InventoryData _inventory;


        public IntProperty Hp = new IntProperty();
        public PerksData Perks = new PerksData();

        public InventoryData Inventory => _inventory;


        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
}

