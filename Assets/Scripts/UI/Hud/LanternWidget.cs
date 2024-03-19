using Creatures.Model.Data;
using Creatures.Player;
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
        private PlayerController _playerController;


        private void Start()
        {
            _session = GameSession.Instance;
            _playerController = FindObjectOfType<PlayerController>();
            _lantern = _playerController.Lantern;

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
                _lantern?.gameObject.SetActive(!_lantern.gameObject.activeSelf);
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
