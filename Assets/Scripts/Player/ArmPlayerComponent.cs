using UnityEngine;

namespace Player
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
