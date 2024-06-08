using UnityEngine;

namespace General.Components.Audio
{
    public class RunSfxWithStartComponent : MonoBehaviour
    {
        [SerializeField] private PlaySfxSound _sound;


        private void Start()
        {
            _sound.Play();
        }


        private void OnEnable()
        {
            _sound.Play();
        }
    }
}
