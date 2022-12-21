using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] private float _value;
        public float Value => _value;

        private float _timesUp;
        public bool IsReady => _timesUp <= Time.time;


        public void Reset()
        {
            _timesUp = Time.time + _value;
        }


        public void SetNewValue(float value)
        {
            _value = value;
        }

    }
}
