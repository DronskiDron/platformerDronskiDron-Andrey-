using Creatures.Model.Data;
using General.Components.GameplayTools;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    public class LanternWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _cooldownImage;
        private GameSession _session;
        private LanternComponent _lantern;

        private static LanternWidget _instance;
        public static LanternWidget I => _instance;


        private void Awake()
        {
            _instance = this;
        }


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _lantern = LanternComponent.I;

            _session.Data.Fuel.OnChanged += UpdateFillAmount;
            UpdateFillAmount(_session.Data.Fuel.Value, 0);

            if (_session.Data.Fuel.Value <= 0)
                OnSwitchLanternWidgetOff();
        }


        private void UpdateFillAmount(int newValue, int oldValue)
        {
            _cooldownImage.fillAmount = (float)newValue / _session.Data.FuelLimit;
            if (newValue <= 0)
                OnSwitchLanternWidgetOff();
        }


        public void OnSwitchOnLight()
        {
            if (_session.Data.Fuel.Value > 0)
                _lantern.gameObject.SetActive(!_lantern.gameObject.activeSelf);
        }


        public void OnSwitchLanternWidgetOn()
        {
            gameObject.SetActive(true);
        }


        public void OnSwitchLanternWidgetOff()
        {
            gameObject.SetActive(false);
        }


        private void OnDestroy()
        {
            _session.Data.Fuel.OnChanged -= UpdateFillAmount;
        }

    }
}
