using Creatures.Model.Definitions;
using Creatures.Model.Definitions.Localisation;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud.BigInventory
{
    public class ItemInfoController : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _text;
        [SerializeField] private CanvasGroup _canvasGroup;


        private void Start()
        {
            NullCheck();
        }


        public void RenewItemInfo(BigInventorySlotWidget slotWidget)
        {
            NullCheck();
            _icon.sprite = slotWidget.Icon.sprite;
            if (slotWidget.Id != "None")
                _text.text = LocalizationManager.I.Localize(GetItemLocalizedName(slotWidget.Id));
            else _text.text = null;

            NullCheck();
        }


        private void NullCheck()
        {
            _canvasGroup.alpha = (_icon.sprite == null) ? 0 : 1;
        }


        private string GetItemLocalizedName(string id)
        {
            var items = DefsFacade.I.Items.All;
            foreach (var item in items)
            {
                if (item.Id == id)
                {
                    return item.LocaleKey;
                }
            }
            return null;
        }
    }
}
