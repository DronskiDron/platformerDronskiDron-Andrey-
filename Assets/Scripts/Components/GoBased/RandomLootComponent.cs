using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace General.Components
{
    public class RandomLootComponent : MonoBehaviour
    {
        [SerializeField] private List<DropCurrency> _lootTable = new List<DropCurrency>();
        [SerializeField][Range(1, 100)] private int _lootDropChance;
        [SerializeField] private int _spawnObjectsCount;
        [SerializeField] private SpawnComponent _spawnComponent;
        [SerializeField] private float _sphereRadius = 0.3f;


        public void CalculateLoot()
        {
            var calcDropChance = Random.Range(0, 101);

            if (calcDropChance > _lootDropChance)
            {
                Debug.Log("Sorry! No loot!");
                return;
            }
            var itemWeight = 0;

            for (int i = 0; i < _lootTable.Count; i++)
            {
                itemWeight += _lootTable[i].DropRarity;
            }

            while (_spawnObjectsCount > 0)
            {
                var randomValue = Random.Range(0, itemWeight);

                for (int j = 0; j < _lootTable.Count; j++)
                {
                    if (randomValue <= _lootTable[j].DropRarity)
                    {
                        _spawnComponent.SpawnRandom(_lootTable[j].Item,
                        transform.position + Random.insideUnitSphere * _sphereRadius);
                        break;
                    }
                    randomValue -= _lootTable[j].DropRarity;
                }
                _spawnObjectsCount -= 1;
            }
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = HandlesUtils.TranspanentGreen;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _sphereRadius);
        }
#endif


        [Serializable]
        public class DropCurrency
        {
            [SerializeField] private string _name;
            [SerializeField] private GameObject _item;
            [SerializeField][Range(1, 100)] private int _dropRarity;

            public string Name => _name;
            public GameObject Item => _item;
            public int DropRarity => _dropRarity;
        }
    }
}

