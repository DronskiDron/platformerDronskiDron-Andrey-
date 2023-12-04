using General.Components.Perks;
using UnityEngine;

namespace Creatures
{
    public class RemainsController : MonoBehaviour
    {
        [SerializeField] ExplosionComponent _explosion;
        [SerializeField] private GameObject[] _goArray;

        
        private void Start()
        {
            _explosion.MultiBlowUp(_goArray);
        }
    }
}
