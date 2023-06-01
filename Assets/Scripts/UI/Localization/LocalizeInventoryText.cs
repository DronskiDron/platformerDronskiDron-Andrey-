using Creatures.Model.Definitions;
using Creatures.Model.Definitions.Localisation;
using Creatures.Model.Definitions.Repository.Items;

namespace UI.Localization
{
    public class LocalizeInventoryText : AbstractLocalizeComponent
    {
        private ItemDef[] _items;


        protected override void Awake()
        {
            _items = DefsFacade.I.Items.ItemsForEditor;
            base.Awake();
        }


        public void OnLocalize()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                _items[i].ItemTitle = LocalizationManager.I.Localize(_items[i].LocaleKey);
            }
        }

        protected override void Localize()
        {

        }
    }
}
