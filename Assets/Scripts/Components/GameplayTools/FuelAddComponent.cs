using Creatures.Model.Data;
using UI.Hud;
using UnityEngine;

namespace General.Components.GameplayTools
{
    public class FuelAddComponent : MonoBehaviour
    {
        [SerializeField] private int _maxFuel;

        private GameSession _session;
        private LanternWidget _lanternWidget;


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _lanternWidget = LanternWidget.I;
        }


        public void AddFuel()
        {
            _session.Data.Fuel.Value = _maxFuel;
            _lanternWidget.OnSwitchLanternWidgetOn();

        }
    }
}
