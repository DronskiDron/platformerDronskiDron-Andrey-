using General.Components.Animation;
using UnityEngine;

namespace Creatures.Boss.Tentacles
{
    public class KillAllEnemiesComponent : MonoBehaviour
    {
        private SpriteAnimation[] _spriteAnimations;


        private void Awake()
        {
            _spriteAnimations = GetComponentsInChildren<SpriteAnimation>();
        }


        public void KillOll()
        {
            foreach (var spriteAnimation in _spriteAnimations)
            {
                if (spriteAnimation == null) continue;
                spriteAnimation?.SetClip("die");
            }
        }
    }
}
