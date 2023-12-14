using Creatures.Model.Data;
using UnityEngine;

namespace General.Components.GameplayTools
{
    public class FuelAddComponent : MonoBehaviour
    {
        [SerializeField] private int _maxFuel;

        private GameSession _session;


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }


        public void AddFuel()
        {
            _session.Data.Fuel.Value = _maxFuel;
        }
    }
}
