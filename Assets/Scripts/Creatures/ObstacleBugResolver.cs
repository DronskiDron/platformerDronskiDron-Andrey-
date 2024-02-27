using System.Collections;
using General.Components.ColliderBased;
using UnityEngine;
using UnityEngine.Events;

namespace Creatures
{
    public class ObstacleBugResolver : MonoBehaviour
    {
        [SerializeField] private LayerCheck _obstacleCheck;
        [SerializeField] private MobAI _mobAI;
        [SerializeField] private float _timeToWait = 1f;
        public bool CoroutineActivated = false;

        public UnityEvent _wasBug;
        private Coroutine _coroutine;


        private void Update()
        {
            if (_obstacleCheck.IsTouchingLayer && _coroutine == null && !_mobAI.IsDead)
            {
                _coroutine = StartCoroutine(StartChecker());
            }
        }


        private IEnumerator StartChecker()
        {
            yield return new WaitForSeconds(_timeToWait);

            if (_obstacleCheck.IsTouchingLayer)
                _wasBug?.Invoke();
            _coroutine = null;
        }
    }
}
