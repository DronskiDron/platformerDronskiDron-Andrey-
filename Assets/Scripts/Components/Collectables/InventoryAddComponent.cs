using Creatures.Model.Definitions;
using Creatures.Player;
using UnityEngine;

namespace General.Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId][SerializeField] private string _id;
        [SerializeField] private int _count;


        public void Add(GameObject go)
        {
            var player = go.GetComponent<PlayerController>();
            player?.AddInInventory(_id, _count);
        }
    }
}
