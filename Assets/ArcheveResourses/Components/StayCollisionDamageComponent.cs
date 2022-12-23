using System.Collections;
using General.Components.ColliderBased;
using UnityEngine;

namespace General.Components
{
    public class StayCollisionDamageComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private EnterEvent _action;
        [SerializeField] private float _secondsToWait = 10f;

        private bool _isCollision = false;
        private bool _firstCollision = true;
        private float _startTime;
        private float _compareTime;
        private int _counter;

        private IEnumerator Wait()
        {
            yield return new WaitUntil(() => _isCollision);
        }


        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(_tag))
            {
                if (_firstCollision)
                {
                    _action?.Invoke(collision.gameObject);
                    _startTime = Time.time;
                    _firstCollision = false;
                }
                else
                {
                    _compareTime = _secondsToWait + _startTime;
                    if (_compareTime <= Time.time)
                    {
                        _action?.Invoke(collision.gameObject);
                        _startTime = Time.time;
                        _counter++;
                        Debug.Log($"Отработал {_counter} раз");
                    }
                    return;
                }
            }
        }
    }
}

