using System;
using UnityEngine;

namespace General.Components.ColliderBased
{
    public class TestEnterCollisionComponent : MonoBehaviour
    {
         [SerializeField] private int _actionUnitsCount;
        [SerializeField] private ActionUnit[] _actionUnit;
        [SerializeField] private EnterEvent _action;


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(_actionUnit[0].Tag))
            {
                _action?.Invoke(collision.gameObject);
            }
        }


        [Serializable]
        public class ActionUnit
        {
            [SerializeField] private string _tag;
            [SerializeField] private EnterEvent _action;

            public string Tag => _tag;
            public EnterEvent Action => _action;
        }
    }
}
