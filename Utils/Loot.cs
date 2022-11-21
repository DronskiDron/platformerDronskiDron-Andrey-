using UnityEngine;

namespace Utils
{
    [CreateAssetMenu]
    public class Loot : ScriptableObject
    {
        public GameObject _lootPrefab;
        public string _lootName;
        public int _dropChance;

        public Loot(string lootName, int dropChance)
        {
            this._lootName = lootName;
            this._dropChance = dropChance;
        }
    }
}
