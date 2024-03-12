using Creatures.Model.Data;
using UnityEngine;
using UnityEngine.Events;

namespace General.Components.Interactions
{
    public class RequireItemComponent : MonoBehaviour
    {
        [SerializeField] private InventoryItemData[] _required;
        [SerializeField] private bool _removeAfterUse;

        [SerializeField] private UnityEvent _onSuccess;
        [SerializeField] private UnityEvent _onFail;


        public void Check()
        {
            var areAllRequirementsMet = true;

            foreach (var item in _required)
            {
                var numItems = GameSession.Instance.Data.Inventory.Count(item.Id);
                if (numItems < item.Value)
                    areAllRequirementsMet = false;
            }

            if (areAllRequirementsMet)
                _onSuccess?.Invoke();
            else
                _onFail?.Invoke();

        }


        public void TakeAwayItem()
        {
            if (_removeAfterUse)
            {
                foreach (var item in _required)
                    GameSession.Instance.Data.Inventory.Remove(item.Id, item.Value);
            }
        }
    }
}
