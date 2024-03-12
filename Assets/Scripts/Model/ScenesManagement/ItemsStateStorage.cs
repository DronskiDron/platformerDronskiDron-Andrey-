using System;
using System.Collections.Generic;
using Creatures.Model.Data.ScenesManagement;
using UnityEngine;

namespace Creatures.Model.Data
{
    [Serializable]
    public class ItemsStateStorage
    {
        [SerializeField] private Dictionary<string, List<string>> _removedItems = new Dictionary<string, List<string>>();


        public void InitDestroyedItemsContainers(ScenesManagementInfo[] scenes)
        {
            foreach (var scene in scenes)
            {
                if (!_removedItems.ContainsKey(scene.Id))
                    _removedItems.Add(scene.Id, scene.GetLevelStoredItems());
            }
        }


        public bool RestoreState(string sceneName, string itemID)
        {
            if (_removedItems.ContainsKey(sceneName))
                return _removedItems[sceneName].Contains(itemID);
            else return false;
        }


        public void StoreState(string sceneName, string itemID)
        {
            if (_removedItems.ContainsKey(sceneName))
                _removedItems[sceneName].Add(itemID);
            else
                _removedItems.Add(sceneName, new List<string> { itemID });
        }


        public void ClearRemoveItemsList(string sceneName)
        {
            if (_removedItems.ContainsKey(sceneName))
                _removedItems[sceneName].Clear();
        }
    }
}
