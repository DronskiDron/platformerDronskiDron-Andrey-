using System;
using Creatures.Model.Definitions.Repository;
using Creatures.Model.Definitions.Repository.Items;
using UnityEngine;

namespace Creatures.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/Repository/Throwable", fileName = "Throwable")]
    public class ThrowableRepository : DefRepository<ThrowableDef>
    {

    }


    [Serializable]
    public struct ThrowableDef : IHaveId
    {
        [InventoryId][SerializeField] private string _id;
        [SerializeField] private GameObject _projectile;

        public string Id => _id;
        public GameObject Projectile => _projectile;
    }
}
