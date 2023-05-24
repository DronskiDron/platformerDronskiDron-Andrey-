using System;
using System.Collections.Generic;
using Creatures.Model.Data.Properties;
using UnityEngine;

namespace Creatures.Model.Definitions.Localisation
{
    public class LocalizationManager
    {
        public readonly static LocalizationManager I;
        private StringPersistentProperty _localeKey = new StringPersistentProperty("en", "localization/current");
        private Dictionary<string, string> _localization;

        public event Action OnLocaleChanged;
        public string LocaleKey => _localeKey.Value;

        static LocalizationManager()
        {
            I = new LocalizationManager();
        }

        public LocalizationManager()
        {
            LoadLocale(_localeKey.Value);
        }


        private void LoadLocale(string localeToLoad)
        {
            var def = Resources.Load<LocaleDef>($"Locales/{localeToLoad}");
            _localization = def.GetData();
            _localeKey.Value = localeToLoad;
            OnLocaleChanged?.Invoke();
        }


        internal string Localize(string key)
        {
            return _localization.TryGetValue(key, out var value) ? value : $"%%%{key}%%%";
        }


        internal void SetLocale(string localeKey)
        {
            LoadLocale(localeKey);
        }
    }
}
