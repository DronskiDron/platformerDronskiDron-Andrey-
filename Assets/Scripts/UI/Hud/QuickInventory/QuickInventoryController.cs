using Creatures.Model.Data;
using UI.Hud.BigInventory;
using UI.Widgets;
using UnityEngine;
using Utils.Disposables;

namespace UI.Hud.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private InventoryItemWidget _prefab;
        [SerializeField] private DeleteFromInventoryComponent _deleteComponent;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private GameSession _session;
        private DataGroup<InventoryItemData, InventoryItemWidget> _dataGroup;


        private void Start()
        {
            _dataGroup = new DataGroup<InventoryItemData, InventoryItemWidget>(_prefab, _container);
            _session = FindObjectOfType<GameSession>();
            _deleteComponent.OnChanged += Rebuild;
            _trash.Retain(_session.QuickInventory.Subscribe(Rebuild));

            Rebuild();
        }


        private void Rebuild()
        {
            var inventory = _session.QuickInventory.Inventory;
            _dataGroup.SetData(inventory);
        }


        private void OnDestroy()
        {
            _trash.Dispose();
            _deleteComponent.OnChanged -= Rebuild;
        }
    }
}
