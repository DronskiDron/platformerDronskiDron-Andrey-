using System.Collections;
using Creatures.Model.Data;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace General.Components.GameplayTools
{
    public class LanternComponent : MonoBehaviour
    {
        [SerializeField] private Light2D _light;
        [SerializeField] private float _goOutPercent = 100f;
        private bool _wasStarted = false;
        private int _startFuel;
        private GameSession _session;

        private static LanternComponent _instance;
        public static LanternComponent I => _instance;


        private void OnEnable()
        {
            _session = FindObjectOfType<GameSession>();
            _startFuel = _session.Data.Fuel.Value;
            if (!_wasStarted)
                StartCoroutine(SpendFuel());
        }


        private void OnDisable()
        {
            _wasStarted = false;
        }


        private void Update()
        {
            if (gameObject.activeSelf && !_wasStarted)
                StartCoroutine(SpendFuel());
        }


        private IEnumerator SpendFuel()
        {
            _wasStarted = true;
            var stepsCount = _startFuel / _goOutPercent;
            var intensity = _light.intensity;
            var radius = _light.pointLightOuterRadius;

            while (_session.Data.Fuel.Value > 0)
            {
                yield return new WaitForSeconds(1);
                _session.Data.Fuel.Value--;

                _light.intensity = (_session.Data.Fuel.Value <= stepsCount) ?
                _light.intensity - (intensity / stepsCount) : intensity;

                _light.pointLightOuterRadius = (_session.Data.Fuel.Value <= stepsCount) ?
                _light.pointLightOuterRadius - (radius / stepsCount) : radius;

                if (_session.Data.Fuel.Value <= 0)
                {
                    _light.intensity = 0;
                    gameObject.SetActive(false);
                    _light.intensity = intensity;
                    _light.pointLightOuterRadius = radius;
                }
            }
        }


        public void InitLantern()
        {
            _instance = this;
        }
    }
}
