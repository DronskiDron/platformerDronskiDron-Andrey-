using Creatures.Model.Data;
using UnityEngine;
using UnityEngine.UI;

namespace PirateIsland.Analytics
{
    public class ToggleAnalyticsComponent : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private InitAnalyticsComponent _initAnalytics;


        private void Start()
        {
            _toggle.isOn = GameSettings.I.Analytics.Value == 1 ? true : false;
        }


        public void OnChangeValue()
        {
            _initAnalytics.SetAnalyticsStatus(_toggle.isOn);
        }
    }
}
