using Creatures.Model.Data;
using UnityEngine;

namespace General.Components.LevelManagement
{
    public class RestoreStateComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        public string Id => _id;

        private GameSession _session;


        private void OnValidate()
        {
            _id = gameObject.name;
        }


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var isDestroyed = _session.RestoreState(Id);
            if (isDestroyed)
                Destroy(gameObject);
        }
    }
}
