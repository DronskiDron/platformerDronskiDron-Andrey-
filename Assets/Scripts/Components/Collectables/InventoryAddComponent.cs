using Creatures.Model.Data;
using Creatures.Model.Definitions.Repository.Items;
using UnityEngine;
using Utils;

namespace General.Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId][SerializeField] private string _id;
        [SerializeField] private int _count;


        public void Add(GameObject go)
        {
            var player = go.GetInterface<ICanAddInInventory>();
            player?.AddInInventory(_id, _count);
        }
    }
}
