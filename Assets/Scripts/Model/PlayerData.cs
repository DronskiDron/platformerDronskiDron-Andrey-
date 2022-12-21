using System;
using UnityEngine;

namespace Creatures.Model
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Hp;
        public int Swords;
        public bool IsArmed;


        public PlayerData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }
}

