using Creatures.Player;
using UnityEngine;

namespace Creatures.Weapons
{
    public class SwordAddComponent : MonoBehaviour
    {
        public void AddSword(GameObject target)
        {
            var playerController = target.GetComponent<PlayerController>();
            playerController?.CollectSwords();
        }
    }
}
