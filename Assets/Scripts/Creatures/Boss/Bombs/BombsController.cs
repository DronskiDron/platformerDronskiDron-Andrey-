using System;
using System.Collections;
using General.Components;
using UnityEngine;

namespace Creatures.Boss.Bombs
{
    public class BombsController : MonoBehaviour
    {
        [SerializeField] private BombSequence[] _sequences;
        private Coroutine _coroutine;


        [ContextMenu("Start bombing")]
        public void StartBombing()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(BombingSequence());
        }


        private IEnumerator BombingSequence()
        {
            foreach (var bombSequence in _sequences)
            {

                foreach (var spawnComponent in bombSequence.BombPoints)
                {
                    spawnComponent.Spawn();
                }
                yield return new WaitForSeconds(bombSequence.Delay);
            }

            _coroutine = null;
        }


        [Serializable]
        public class BombSequence
        {
            [SerializeField] private SpawnComponent[] _bombPoints;
            [SerializeField] private float _delay;

            public SpawnComponent[] BombPoints => _bombPoints;
            public float Delay => _delay;
        }
    }
}
