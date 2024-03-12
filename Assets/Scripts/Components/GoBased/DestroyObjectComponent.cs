using System;
using System.Collections;
using Creatures.Model.Data;
using General.Components.LevelManagement;
using UnityEngine;

namespace General.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        [SerializeField] private RestoreStateComponent _state;
        [SerializeField] private float _delayValue = 1f;


        private void StateCheck()
        {
            var session = GameSession.Instance;
            if (_state != null)
                session.ItemStateStorage.StoreState(session.GetCurrentSceneName(), _state.Id);
        }


        public void DestroyObject()
        {
            Destroy(_objectToDestroy);
            StateCheck();
        }


        public void DestroyWithDelay()
        {
            StartCoroutine(WaitAndDestroy());
        }


        private IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(_delayValue);
            Destroy(_objectToDestroy);
            StateCheck();
        }


        public void DestroyCurrent(GameObject go)
        {
            Destroy(go);
            StateCheck();
        }

    }
}
