using System.Collections;
using UnityEngine;

namespace Creatures.Boss
{
    public class FloodController : MonoBehaviour
    {
        [SerializeField] private Animator _floodAnimator;
        [SerializeField] private float _floodTime;

        private static readonly int IsFlooding = Animator.StringToHash("IsFlooding");

        private Coroutine _coroutine;


        public void StartFlooding()
        {
            if (_coroutine != null) return;

            _coroutine = StartCoroutine(Animate());
        }


        private IEnumerator Animate()
        {
            _floodAnimator.SetBool(IsFlooding, true);
            yield return new WaitForSeconds(_floodTime);
            _floodAnimator.SetBool(IsFlooding, false);
            _coroutine = null;
        }


        public void StopFlooding()
        {
            // StopCoroutine(Animate());
            _floodAnimator.SetBool(IsFlooding, false);
            // _floodAnimator.Play("Hide");
        }
    }
}
