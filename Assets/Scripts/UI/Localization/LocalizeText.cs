using Creatures.Model.Definitions.Localisation;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Localization
{
    [RequireComponent(typeof(Text))]
    public class LocalizeText : AbstractLocalizeComponent
    {
        [SerializeField] private string[] _keys;
        [SerializeField] private bool _capitalize;

        private Text _text;


        protected override void Awake()
        {
            _text = GetComponent<Text>();
            base.Awake();
        }


        protected override void Localize()
        {
            foreach (var key in _keys)
            {
                var localized = LocalizationManager.I.Localize(key);
                _text.text = _capitalize ? localized.ToUpper() : localized;
            }
        }
    }
}
