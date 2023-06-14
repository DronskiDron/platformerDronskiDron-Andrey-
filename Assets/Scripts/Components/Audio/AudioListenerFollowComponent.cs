using Creatures.Player;
using UnityEngine;

namespace General.Components.Audio
{
    public class AudioListenerFollowComponent : MonoBehaviour
    {
        private Transform _target;


        private void Start()
        {
            _target = FindObjectOfType<PlayerController>().gameObject.transform;
            SetAudioListenerPosition();
        }


        private void Update()
        {
            SetAudioListenerPosition();
        }


        public void SetAudioListenerPosition()
        {
            transform.position = new Vector3(_target.position.x, _target.position.y, transform.position.z);
        }
    }
}
