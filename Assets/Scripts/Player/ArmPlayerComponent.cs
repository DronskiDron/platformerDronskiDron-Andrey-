using Creatures.Player;
using UnityEngine;

namespace Creatures
{
    public class ArmPlayerComponent : MonoBehaviour
    {
        public void ArmPlayer(GameObject go)
        {
            var player = go.GetComponent<PlayerController>();
            player?.ArmPlayer();
        }
    }
}
