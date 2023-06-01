using UnityEngine;
using System;
using System.Linq;
using Creatures.Model.Definitions.Items;

namespace Creatures.Model.Definitions.Repository.Items
{
    [CreateAssetMenu(menuName = "Defs/Items", fileName = "Items")]
    public class ItemsRepository : DefRepository<ItemDef>
    {

#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _collection;
#endif

    }


    [Serializable]
    public struct ItemDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private string _itemTitle;
        [SerializeField] private string _localeKey;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemTag[] _tags;

        public string Id => _id;
        public string ItemTitle { get => _itemTitle; set => _itemTitle = value; }
        public string LocaleKey { get => _localeKey; set => _localeKey = value; }


        public bool IsVoid => string.IsNullOrEmpty(_id);
        public Sprite Icon => _icon;


        public bool HasTag(ItemTag tag)
        {
            return _tags?.Contains(tag) ?? false;
        }
    }
}
